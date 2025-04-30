# This is a collection of tools to help me identify coupling in code bases


Coupling is a necessary property of any information system.
However we can control the type, degree and distance of coupling

If we can visualise and identify things that we care about we can analytiically identify targets for refactors.

I intend to keep this as simple as possible.

Because it is nearly impossible for this to eork accross multiple repositories without additional information to connect things up the value of any of these tools is severley limited without also having an additional system to supply that data.

the main info i want to get is team composition of the comitters accross code bases at different points in time. This will really help to identify the cross team coupling.
In theory it is fine for multiple teams to be in one code base - as long as they are not tripping over each other - if they are then there is either a code refactor or team refactor needed (or an acceptance that really there is only 1 team).

These are the kinds of problems that are often not asked nor answered in organisations.
They are also the kinds of problems that I believe will have an order of magnitude more effect on throughput than any amount of harassment from a delivery / product manager.

## Projects

- the dotnet project is a set of tools designed to analyse a git log to extract data
- the front end is a set of visualisation tools - these tools will need parameters tweaking and the layout algorithms are not very well optomised (and so redraw on very large graphs will be slow - seconds)
