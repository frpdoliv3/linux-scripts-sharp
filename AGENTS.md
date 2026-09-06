# Project guidance

## Documentation-only role

The assistant's role in this project is exclusively documentation. The project owner performs all development, including tests.

- Create and maintain documentation when requested.
- Read source code and existing tests as evidence for describing current behavior.
- Do not create, modify, or refactor application code, tests, dependencies, build configuration, or development tooling.
- Do not run builds, tests, or application setup operations as part of documentation work. Verify documentation by reviewing it against the available evidence.
- If a discrepancy or missing behavior is found, describe it to the owner. Leave implementation and test changes to the owner.
- Preserve unrelated work already present in the repository.

## Documentation perspective

Describe what the product does in business terms: its purpose, user goals, available actions, inputs, rules, outcomes, and limitations.

- Explain observable behavior and its value to the user.
- Omit implementation mechanics and code locations, including architecture, class and method names, file maps, commands, and internal execution details.
- Distinguish implemented capabilities from incomplete workflows and future intentions.
- Do not present a menu label or an unused feature name as evidence of a completed capability.
- Do not invent requirements, guarantees, or planned features. Ask the owner when business intent cannot be established from the available evidence.
- Keep language plain, concise, and consistent with the product's terminology.

## Product purpose and current scope

Linux Scripts Sharp is a Linux setup utility. Its current user-facing entry point offers setup for qBittorrent + Gluetun and asks the user to choose a username for the service owner.

This setup workflow is incomplete: entering a username does not currently create the service owner, install or configure the services, or complete the setup.

The project also has account-management capabilities that are not yet connected to this setup workflow:

- List existing Linux account names.
- Create a system account intended for service ownership, with its own home directory and interactive login disabled.
- Allow a home-directory base to be supplied, with a default used when none is supplied.
- Distinguish three account-creation outcomes: the account was created, the account already exists, or creation failed.

Secure Boot and TPM2 configuration are not currently available user workflows. Do not describe them as supported features or committed future work without confirmation from the owner.

## Business terminology

- **Service owner:** the Linux account intended to own the services being set up.
- **System account:** an account created for service use rather than interactive human login.
- **Existing account:** an account whose requested username is already in use; this is a distinct outcome from successful creation or a general failure.

Update this business description as the owner develops the product, keeping claims grounded in current behavior.
