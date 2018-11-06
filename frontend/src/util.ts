import moment from 'moment'
import _marked from 'marked'
import _readingTime from 'reading-time'
import _htmlEllipsis from 'html-ellipsis'

export function fuzzyDate(date: Date) {
    let dm = moment(date);
    let lastMonth = moment().subtract(1, 'month');
    if(dm.isBefore(lastMonth)) {
        return formatDate(date);
    }
    return dm.fromNow();
}

export function marked(text: string): string {
    return _marked(text, {
        breaks: true
    });
}

export function formatDate(date: Date) {
    return moment(date).format('lll');
}

interface ReadingTimeStats {
    text: string;
    words: number;
}

export function readingTime(text): ReadingTimeStats {
    return _readingTime(text);
}

export function htmlEllipsis(text: string, length: number, includeEllipsis: boolean = true): string {
    return _htmlEllipsis(text, length, includeEllipsis);
}