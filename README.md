# PomodoroClock
A simple and nice scaleable timer application, which counts in 25 minutes intervals. Usefull for "Pomodoro" time management.

-----

I really like the Pomodoro Technique (http://pomodorotechnique.com/). It’s a simple time management method which helps you getting things done (GTD), stay focused and be more productive. For this you need a timer which counts 25 Minutes of Work and 5 Minutes of Breaks.
https://en.wikipedia.org/wiki/Pomodoro_Technique
I like to use a Pomodoro Timer when doing focused work on my computer.
And I don’t like software where I have to do more clicks then absolute necessary (i.e. Power User). Therefore I had some requirements for a Timer Software:
## Personal Timer Requirements
* Since I’m managing my Pomodoros via good old fashioned pen & paper, I don’t want to be urged to specifiy tasks in the timer application. I just want to start and stop it.
* I want to have a clear visual feedback how much time I have still left. So in addi-tion to a digital clock a more visual way (e.g. color, graph) would be nice.
* The timer needs always to stay in my sight. Otherwise I wouldn’t get into the Eustress (Positive Stressor) phase as good. Therefore it has to stay on top of oth-er applications.
* Since the timer always stays on top, it has to be small in it’s dimensions.
* It has to run on a Windows PC, preferably without installing strange frameworks. 
## Result 
I searched on the internet for some while but didn’t find anything which suited me and my requirements. And so I wrote a simple C# Application for me. Perhaps it helps some out there with similar problems. 
## Software-Features
* One Click Start. After 25 Minutes have passed another click starts the Break. 
* App is highly (geometric) scaleable and can be “parked” in free screen areas
* Visual Feedback with color changing time bar
* Stays always on top of other applications
* Remembers Window Position and Size upon next start.
* Built with .Net 4.5

## Remarks
Software doesn’t play a ringing sound after the Pomodoro / Break. I find this disturbing in an office environment.   
