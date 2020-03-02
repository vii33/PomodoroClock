Website Version:  https://derveit.github.io/PomodoroClock/

-----

# PomodoroClock
A lean desktop timer application, which counts 25 minutes intervals. Useful for the "Pomodoro" time management technique.

-----

## Introduction
I really like the [Pomodoro Technique](http://pomodorotechnique.com/). 
It’s an easy to learn time management method which helps you getting things done (GTD), stay focused and be more productive. 

https://en.wikipedia.org/wiki/Pomodoro_Technique

For this method you need a timer which counts 25 minutes of work and 5 minutes of breaks.
I wanted to use a Pomodoro Timer while doing work on my computer but I didn't find a suitable application, so I wrote one.

## Software Requirements
. https://dotnet.microsoft.com/download/dotnet-framework


. And I'm also kind of a Power User who tweaks every software to achieve a maximum work-flow output. 
Therefore I had some requirements for a Timer Software:

## Personal Timer Requirements
* Since I’m managing my Pomodoros via good old fashioned pen & paper, I don’t want to be urged to specify tasks in the timer application. I just want to start and stop it.
* I want to have a clear visual feedback of how much time I have still left. So in addition to a digital clock a more visual way (e.g. color) would be nice.
* The timer needs always to stay in my sight. Otherwise I wouldn’t get into the Eustress (Positive Stressor) phase as good. Therefore it has to stay on top of other applications.
* Since the timer always stays on top, it has to be small in it’s dimensions, moveable and scaleable.
* It has to run on a Windows PC, preferably without installing strange frameworks. 

## Result 
I searched on the internet for some while and tried different desktop and web-based timers, but I didn’t find anything which suited my requirements. 
And so I wrote a simple C# application. I make this project public, perhaps the software can help some with similar problems. 

## Software-Features
* One Click Start. After 25 minutes have passed, another click starts the break. 
* App is highly (geometric) scalable and can be “parked” in free areas anywhere on the screen.
* Visual Feedback with color changing time bar
* Stays always on top of other applications
* Remembers window position and size upon next start.
* Also shows progression in task bar
* Built with .Net 4.5

## Remarks
Software doesn’t play a ringing sound after the Pomodoro / Break. Perhaps I'll add this feature later.  

*If the software helps you or you have some questions, feel free to contact me.*

## Acknowledgement
Tomato Icon made by <a href="http://www.flaticon.com/authors/roundicons" title="Roundicons">Roundicons</a> from <a href="http://www.flaticon.com" title="Flaticon">www.flaticon.com</a> is licensed by <a href="http://creativecommons.org/licenses/by/3.0/" title="Creative Commons BY 3.0" target="_blank">CC 3.0 BY</a></div>
