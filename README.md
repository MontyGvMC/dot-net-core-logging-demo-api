# .NET Core Logging Demo API
A simple playground to figure out how collect the interesting data when writing logs in a .NET Core web API

**One note right at the beginning!**

This logging demo is **NOT** about implementing custom logger, loggerfactories etc.

## What is it about?

.NET Core provides us the Microsoft.Extensions.ILogger interface which we can use for writing our logs. And there are a bunch of logging frameworks which from which we can choose in order to decide where our logs are going to.

Now it is up to us to take the ILogger, grab the data we want to log and write it.

And this tiny API is about to do exactly that because it sometimes gets tricky to get the data/information we want and it is also depends on the preferences of the developer where to place the code to write the logs.

Questions we handle
 * where to place the code?
   * in which classes?
   * where in the pipeline?
  * how and from where do we get the information that is of interest in that location?
  
## Intention

This is not a full library. It's just a playground and demo where people can look up how to do logging related things.

## The preferences

 * Centralized: Of course we can most of our logs write directly into our action but this clutters our (controller) code so let's figure out what we can centralize.
 * seperation of concern: we give it a try to implement the logging code to do the logging and nothing else (so let's avoid mixing creating responses and log them)
 * Security: I heard the statement "I do not alllow my developers to log that extensively because it could leak sensitive information". So lets experiment how to hide those sensitive information.
  * log load: in a perfectly error free product we may actually don't wan't/need logs at all. In cases where we have logs we don't want du have the same information in multiple log entries.

## What we want to log?

A typical statement we stumble upon is "it depends on your requirements."

So in this repo we will define some scenarios (mostly some error cases) and figure out how to produce the logs helping us to identify the problems.

