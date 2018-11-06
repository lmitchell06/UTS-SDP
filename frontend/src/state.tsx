import linq from 'linq'
import API, { APIUser, APIJournal, APIEntry, APIRevision } from './api'

// if there's an unhandled exception, we catch it and set our status to error.
// this means we don't ever need to handle errors on API requests -- we simply prompt the user to refresh.
let originalUnhandled = (Promise as any)._unhandledRejectionFn;
(Promise as any)._unhandledRejectionFn = function(e) {
    State.getState().statusState = StatusState.Error;
    originalUnhandled(e);
}

export enum LoginState {
    Loading = "LOADING", In = "IN", Out = "OUT"
}

export enum StatusState {
    Ok = "OK", Error = "ERROR"
}

export class State {
    loginState = LoginState.Loading;
    statusState = StatusState.Ok;

    journalEntriesShowHidden = false;

    currentUser = new User();
    
    protected constructor() {
        (window as any).STATE = this;
        (window as any).linq = linq;
        // get the initial state
        this.refreshUser();
    }

    async refreshUser() {
        let user = await API.getCurrentUser();
        if(user != null) {
            this.transformUser(user);
            this.loginState = LoginState.In;
            await this.updateJournals();
        } else {
            this.loginState = LoginState.Out;
        }
    }

    async login(Username: string, Password: string) {
        if(this.loginState == LoginState.Loading) return;

        let loginResponse = await API.login({
            Username, Password
        });
        
        if(loginResponse.User != null) {
            // logged in
            this.transformUser(loginResponse.User);
            this.loginState = LoginState.In;
            await this.updateJournals();
        } else {
            throw loginResponse.ValidationResults.filter(result => !result.IsValid).map(result => result.Message);
        }

        /*this.currentUser.id = 1;
        this.currentUser.journals.push(
            new Journal(2, "Awesome Project", [
                new Entry(3, [
                    new Revision(4, 3, new Date(), "Hello this is a revision")
                ], "Entry Name", new Date(), 2, 1, false)
            ])
        );*/
    }

    async logout() {
        try {
            await API.logout();
        }
        catch(e) {
            console.warn(`tried to log out, but couldn't. making it look like I logged out anyway.`);
        }

        this.loginState = LoginState.Out;
    }

    async updateJournals() {
        if(this.loginState != LoginState.In) return;

        let journals = await API.getJournals();
        journals.forEach(journal => this.transformJournal(journal));
    }

    async register(FirstName, LastName, Username, Password, Company) {
        let response = await API.registerUser({
            FirstName,
            LastName,
            Username,
            Password,
            Company,
            DateOfBirth: new Date()
        });
        if(response.UserId > 0) {
            // all good! let's do the standard login now, but there'd be no journals yet.
            await this.login(Username, Password);
            return true;
        }
        return response.ValidationResults;
    }

    async createEntry(ParentId, Title, Body) {

        let journal = this.currentUser.journals.find(j => j.id == ParentId);
        if(!journal) throw 'no journal';

        let response = await API.createEntry({ 
            ParentId,
            Title,
            Body
        });

        // ok!
        this.transformEntry(journal, response);

        return response.Id;
    }

    async createRevision(ParentId, EntryId, Body) {
        let journal = this.currentUser.journals.find(j => j.id == ParentId);
        if(!journal) throw 'no journal';
        let entry = journal.entries.find(e => e.id == EntryId);
        if(!entry) throw 'no entry';

        let response = await API.editEntry({
            EntryId,
            Body
        });

        // ok!
        this.transformRevision(entry, response);
    }

    async createJournal(ProjectName) {
        let journal = await API.createJournal({
            ProjectName
        });

        this.transformJournal(journal);

        return journal.JournalId;
    }

    async hideEntry(entry: Entry) {
        let change = !entry.hidden;
        await API.toggleEntryHidden({
            EntryId: entry.id,
            Hidden: change
        });

        entry.hidden = change;
    }

    async deleteEntry(entry: Entry) {
        await API.deleteEntry(entry.id);
        // entry is deleted, remove it from (my) list
        let journal = entry.parentJournal;
        if(journal.parentUser === this.currentUser)
            journal.entries.splice(journal.entries.indexOf(entry), 1);
    }

    private transformUser(apiUser: APIUser) {
        if(this.currentUser.id != apiUser.Id)
            this.currentUser.journals = [];
        this.currentUser.id = apiUser.Id;
        this.currentUser.name = `${apiUser.PersonalDetails.FirstName} ${apiUser.PersonalDetails.LastName}`;
    }

    private transformJournal(apiJournal: APIJournal) {
        // try to find existing journal in the array
        let journal = this.currentUser.journals.find(j => j.id == apiJournal.JournalId);
        if(journal == undefined) {
            // didn't find it - create a new one
            journal = new Journal(this.currentUser);
            this.currentUser.journals.push(journal);
        }
        // now, update it
        journal.projectName = apiJournal.ProjectName;
        journal.id = apiJournal.JournalId;
        journal.authorId = apiJournal.Journal.AuthorId;
        apiJournal.Journal.Entries.forEach(entry => this.transformEntry(journal, entry));
    }

    private transformEntry(journal: Journal, apiEntry: APIEntry) {
        let entry = journal.entries.find(e => e.id == apiEntry.Id);
        if(entry == undefined) {
            entry = new Entry(journal);
            journal.entries.push(entry);
        }
        entry.id = apiEntry.Id;
        entry.parentId = journal.id;
        entry.authorId = journal.authorId;
        entry.title = apiEntry.Title;
        entry.created = new Date(apiEntry.Created);
        entry.hidden = apiEntry.Hidden;
        apiEntry.Revisions.forEach(revision => this.transformRevision(entry, revision));
    }

    private transformRevision(entry: Entry, apiRevision: APIRevision) {
        let revision = entry.revisions.find(r => r.id == apiRevision.Id);
        if(revision == undefined) {
            revision = new Revision(entry);
            entry.revisions.push(revision);
        }
        revision.id = apiRevision.Id;
        revision.body = apiRevision.Body;
        revision.parentId = entry.id;
        revision.edited = new Date(apiRevision.Edited);
    }

    // singleton
    private static s: State = undefined;
    static getState() {
        if(this.s === undefined) this.s = new State();
        return this.s;
    }
}

export class User {
    id = -1;
    name = "";

    journals: Journal[] = [];
}

export class Journal {
    constructor(
        public parentUser: User,
        public id = -1,
        public projectName = "",
        public entries: Entry[] = [],
        public ownerId = -1,
        public authorId = -1
    ) {}

    get lastModified(): Date {
        let latest = this.latestEntry;
        if(latest) return latest.lastModified;
        return new Date();
    }

    get latestEntry(): Entry {
        return linq.from(this.entries)
            .orderByDescending(e => e.lastModified)
            .firstOrDefault();
    }
}

export class Entry {
    constructor(
        public parentJournal: Journal,
        public id = -1,
        public revisions: Revision[] = [],
        public title = "",
        public created = new Date(0),
        public parentId = -1,
        public authorId = -1,
        public hidden = false
    ) {}

    get latestRevision(): Revision {
        return linq.from(this.revisions).orderByDescending(x => x.edited).firstOrDefault();
    }

    get lastModified(): Date {
        let lastRev = this.latestRevision;
        if(lastRev == null) return this.created;
        return lastRev.edited;
    }
}

export class Revision {
    constructor(
        public parentEntry: Entry,
        public id = -1,
        public parentId = -1,
        public edited = new Date(0),
        public body = ""
    ) {}
}