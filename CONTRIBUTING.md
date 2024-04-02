# Contributing to the Helldivers 2 Community API

First of all, thank you very much for taking the time to contribute to this community driven API!
If you aren't already, feel free to take a peek in [Helldivers 2 Developers Discord](https://discord.gg/E8UUfWYmf9).

Let's run you through the process of contributing to the community API.

## Create an issue if none exist

The first step to contributing is making sure that your contribution will be accepted.
Opening a [bug](https://github.com/helldivers-2/api/issues/new/choose)
or [feature request](https://github.com/helldivers-2/api/issues/new/choose)
(that can potentially be transformed into a discussion) will ensure that the relevant maintainers are aware something
will be worked on. This ensures that they aren't working on the same issue you are, or in the case of a feature request,
that we are open to implementing the feature
(see [Why we might not accept all Pull Requests](#why-we-might-not-accept-all-pull-requests)).
Once a maintainer (most notably @dealloc and/or @lambstream) approves your change request, you can get started.

## Fork the API
See [Github's fork documentation](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/working-with-forks/fork-a-repo).

## Set up a local development environment
(TODO: expand on all required dependencies, setup etc)
Once you have cloned your fork or the API itself, run the following commands to initialize the local environment:
```shell
git submodule update .\src\Helldivers-2-Models\json
dotnet restore
dotnet build --no-restore --no-incremental
```
If you don't get any errors, that means your development environment is all set to get developing!
Open the `Helldivers-2.sln` file in the root of the repository and you're good to go!

### Formatting, line endings and syntax
There's three pilars when it comes to coding conventions, formatting etc:
- [.editorconfig](https://editorconfig.org/) is set up in the repository, if your IDE supports it you should install
    the required plugin to ensure your code is correctly formatted according to it.
- [Microsoft C# coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
    write idiomatic C# and try to follow conventions from either Microsoft or the project if it deviates from a specific
    rule.
- [dotnet-format](https://github.com/dotnet/format) we use `dotnet format` in our CI pipeline to ensure consistent
    formatting, line endings and syntax. Before submitting a pull request, make sure you run `dotnet format` on your code.

### Be respectful
Disagreements happen, discussions are perfectly fine (we encourage a healthy discussion even!), however you (and we) are
to remain respectful at **all times**. While we try to listen to the community whenever possible, please keep in mind
that 1. we do this in our free time (and for free nonetheless), and 2. we have the *final* say when it comes to decisions
regarding the code that we maintain. See [Code of conduct](https://github.com/helldivers-2/api?tab=coc-ov-file) for more
information on our expectations on the behaviour of our maintainers and community.

### Licensing
At the time of writing our project is licensed under [MIT](https://en.wikipedia.org/wiki/MIT_License) (you can read the
'plain English' version [here](https://www.tldrlegal.com/license/mit-license)).
When you contribute to this project you accept that your code will also be licensed under MIT, and that you give full
ownership of the contributed code to the [helldivers-2/api](https://github.com/helldivers-2/api) project and it's
maintainers. Under no circumstance are you entitled to compensation in any form, nor entitled to more involvement or
authority over the code in this project, both contributed and otherwise.

## Why we might not accept all Pull Requests
There are three primary reasons we might choose not to accept your pull request (however it's not limited to these two);
1. **We don't accept pull requests that are cosmetic in nature** (think formatting, whitespace, ...)
    we don't do this for the same reason the [Ruby Rails](https://github.com/rails/rails) project does not allow such
    changes, [TLDR](https://github.com/rails/rails/pull/13771#issuecomment-32746700) it adds noise and makes maintenance
    even more difficult than it already is.
2. **We don't accept pull requests with features we don't want/feel comfortable maintaining**
    If you create an awesome feature that is rather complex, we might choose not to accept the feature.
    This is probably not related to the quality of your code (that's what PR's are for in the first place), but rather
    with the fact that after you've left the maintainers still need to make sure it keeps working for as long as we
    don't sunset the feature. If your feature/PR would add too much of a burden on the maintenance we might choose not
    to accept it, regardless of how cool or convenient it is (sorry!).
3. **It falls outside the scope of this project**
    This project aims to be a wrapper API around the official Helldivers API by ArrowHead studios. While there *might*
    be exceptions, any information not available in the API (or the game) falls outside the scope, the same goes for
    endpoints that might be considered dangerous (for example those that require authentication on the ArrowHead API).
