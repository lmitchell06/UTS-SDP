var shortDelay = () => new Promise(resolve => setTimeout(resolve, Math.random() * 500)); // 0 - 500ms
var longDelay = () => new Promise(resolve => setTimeout(resolve, 1500 + Math.random() * 500)); // 1.5s - 2s

import {
    User,
    LoginWithCredentialsRequest,
    LoginWithCredentialsResponse,
    RegisterNewUserRequest,
    RegisterNewUserResponse,
    ViewJournalRequest,
    ViewJournalResponse,
    ListEntriesRequest,
    JournalEntry,
    CreateNewJournalRequest,
    CreateEntryRequest,
    EditEntryRequest,
    Revision,
    ToggleHiddenRequest,
    HideJournalRequest,
    SearchEntryRequest,
    LoginApi,
    EntryApi,
    UserApi,
    JournalApi,
    RegisterApi,
} from './generated'

export {
    User as APIUser,
    ViewJournalResponse as APIJournal,
    JournalEntry as APIEntry,
    Revision as APIRevision
} from './generated'

// a more saner interface than the real api, but still a bit insane

interface API {
    login(request: LoginWithCredentialsRequest): Promise<LoginWithCredentialsResponse>;
    logout(): Promise<void>;

    getCurrentUser(): Promise<User>;
    registerUser(request: RegisterNewUserRequest): Promise<RegisterNewUserResponse>;

    getJournals(): Promise<ViewJournalResponse[]>;
    getJournal(request: ViewJournalRequest): Promise<ViewJournalResponse>;

    getEntries(request: ListEntriesRequest): Promise<JournalEntry[]>;
    createEntry(request: CreateEntryRequest): Promise<JournalEntry>;
    editEntry(request: EditEntryRequest): Promise<Revision>;
    toggleEntryHidden(request: ToggleHiddenRequest): Promise<boolean>;
}

class RealAPI implements API {
    private loginApi: LoginApi;
    private userApi: UserApi;
    private entryApi: EntryApi;
    private journalApi: JournalApi;
    private registerApi: RegisterApi;

    constructor(private baseUrl?: string) {
        this.userApi = new UserApi(undefined, this.baseUrl);
        this.entryApi = new EntryApi(undefined, this.baseUrl);
        this.journalApi = new JournalApi(undefined, this.baseUrl);
        this.registerApi = new RegisterApi(undefined, this.baseUrl);
        this.loginApi = new LoginApi(undefined, this.baseUrl);
    }

    login(request: LoginWithCredentialsRequest) {
        return this.loginApi.loginLoginWithCredentials({ request });
    }
    logout() {
        return this.loginApi.loginLogout();
    }
    getCurrentUser() {
        return this.userApi.userGetCurrentUser();
    }
    registerUser(request: RegisterNewUserRequest) {
        return this.registerApi.registerRegisterNewUser({ request });
    }
    userExists(username: string) {
        return this.userApi.userUserExists({ username });
    }

    getJournals(showHidden: boolean = true) {
        return this.journalApi.journalListJournals( { showHidden });
    }
    getJournal(request: ViewJournalRequest) {
        return this.journalApi.journalViewJournal({ request });
    }
    createJournal(request: CreateNewJournalRequest) {
        return this.journalApi.journalCreateJournal({ request });
    }
    archiveJournal(request: HideJournalRequest) {
        return this.journalApi.journalHideJournal({ request });
    }
    
    getEntries(request: ListEntriesRequest) {
        return this.entryApi.entryListEntries({request});
    }
    createEntry(request: CreateEntryRequest) {
        return this.entryApi.entryCreateNewEntry({ request });
    }
    editEntry(request: EditEntryRequest) {
        return this.entryApi.entryEditEntry({ request });
    }
    toggleEntryHidden(request: ToggleHiddenRequest) {
        return this.entryApi.entryToggleHidden({ request });
    }
    deleteEntry(entryId: number) {
        return this.entryApi.entryDeleteEntry({ entryId });
    }
    searchEntries(request: SearchEntryRequest) {
        return this.entryApi.entrySearchEntries({ request });
    }
}

export default new RealAPI();