{ pkgs ? import <nixpkgs> {} }:

with pkgs;
let
  inputs = [
    curl
    elixir_1_14
    flyctl
    gcc
    glibcLocales
    gnumake
    libiconv
    libxml2
    openssl
    postgresql
    readline
    yarn
    zlib
  ] ++ lib.optional stdenv.isLinux inotify-tools
    ++ lib.optional stdenv.isDarwin terminal-notifier
    ++ lib.optionals stdenv.isDarwin (with darwin.apple_sdk.frameworks; [
        CoreFoundation
        CoreServices
      ]);

  # define shell startup command
  hooks = ''
    # this allows mix to work on the local directory
    mkdir -p .nix-mix
    mkdir -p .nix-hex
    export MIX_HOME=$PWD/.nix-mix
    export HEX_HOME=$PWD/.nix-hex
    export PATH=$MIX_HOME/bin:$PATH
    export PATH=$HEX_HOME/bin:$PATH
    export LANG=en_US.UTF-8
    export ERL_AFLAGS="-kernel shell_history enabled"
  '';

in mkShell {
  buildInputs = inputs;
  shellHook = hooks;
}
