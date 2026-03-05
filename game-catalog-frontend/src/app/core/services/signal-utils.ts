import { input, Signal } from "@angular/core";
import { toObservable,toSignal } from "@angular/core/rxjs-interop";
import { debounceTime } from "rxjs/operators";
export function debounceSignal<T>(input: Signal<T>, delay: number, initialValue: T): Signal<T> {

    const signalObs = toObservable(input);
    return toSignal(signalObs.pipe(debounceTime(delay)), { initialValue });
}
// followed this  https://www.reddit.com/r/angular/comments/1jo61m7/the_easiest_way_to_implement_debounce_with/   
// explaination what it does 
// This function takes a signal and then converts it into an observable to apply the debounceTime operator, (only available in observable Rxjs) and then convert it into the signal
     