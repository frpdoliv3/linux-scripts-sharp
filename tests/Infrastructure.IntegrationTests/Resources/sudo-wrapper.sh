#!/bin/sh

/usr/bin/sudo -k

printf '%s\n' "$TEST_PASSWORD" |
    /usr/bin/sudo -S -p '' "$@"
