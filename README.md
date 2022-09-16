# PrimeCalculator

## Disclaimer
This codebase is made for self-teaching and educational purposes only.
Many features like input validation, object disposed checks, some exception handling, etc... are mostly missing.
As such this codebase cannot be considered production ready.

## What's this ?
This is a simple WPF app implementing a parallelized prime factorization program leveraging async programming.
This project has been manly a case study to learn more about the WPF framework, parallel programming, async programming and cancellation.  
The cancellation is done not using the classic CancellationTokenSource and CancellationToken pattern as at the time I just started learning C# and didn't really know about it :)

## How does it work ?
It's easy to make a parallel program that factorizes a series of numbers. The tricky part is to keep the factorization parallel while displaying the results sequentially.
We also have to take into account that we're running inside a GUI so we must avoid any long running operation that would hang the event loop. 
We must also fit in a meaningful "cancellation" for the whole operation.

In order to accomplish all of this we need multiple async parallel tasks:
* #### An async task that will spawn all the single number factorization tasks 
	This is needed to not hang the GUI and to let the printing task start immediately because spawning all the tasks for a big range would require a lot of time.
*  #### A task for each number in the range that will factorize it
	This is self explanatory
* #### An async task that will sequentially print all the resulsts
	This will concurrently execute with the 'tasks spawning' task. In order to synchronize the two of them a semaphore is used. The printing task will advance only after a new task is created, this is to avoid waiting for 'null' tasks.


The application is structered using sort of a MVP pattern (Model-View-Presenter) omitting the IView interface and working directly on the Window object. The Window doesn't know anything about the App itself as the "commands" to execute are subscribed by the App to the Window's events. 
Another strategy could have been to pass Command objects to the Window constructor.


## How should I use this ?
Just insert a number range to factorize and press "Calculate". If you want to stop the operation press "Stop". To showoff the real potential of the app try to insert a big range and/or start from a big number. "Change language" was put there only as a case study for WPF resources.






