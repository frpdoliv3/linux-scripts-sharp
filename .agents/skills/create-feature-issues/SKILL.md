---
name: create-feature-issues
description: Create one or more GitHub issues from feature or requirement descriptions provided by the user.
---

# Create GitHub Issues

Create one or more GitHub issues from the user's requirements.

All issues must follow `.github/ISSUE_TEMPLATE/feature.yaml`.

## Workflow

1. Read the user's requirements.

2. Determine whether the request describes one cohesive feature or multiple independently actionable features.
   - Create one issue for a cohesive feature.
   - Create separate issues for independently actionable features.

3. Verify factual claims about the existing system against the codebase.
   - Inspect the code only when necessary to verify such claims.
   - Do not use the codebase to redesign, improve, reinterpret, or complete the user's feature description.

4. If a factual claim contradicts the codebase:
   - Do not correct or rewrite the feature description.
   - Do not create the affected issue.
   - Explain which claim appears incorrect.
   - Point the user to the relevant code.
   - Wait for the user to revise the requirements and invoke the skill again.

5. For each issue, verify that the user's prompt explicitly or implicitly answers every required field in `.github/ISSUE_TEMPLATE/feature.yaml`:
   - Type
   - Description
   - Motivation
   - Acceptance criteria

6. If a required field cannot be determined from the user's prompt:
   - Do not invent the missing information.
   - Do not create the affected issue.
   - Ask the user for the missing information.

7. Fill optional fields only when relevant information is present:
   - Requirements
   - Technical notes
   - Additional context

8. Check existing open issues for obvious duplicates.
   - If an equivalent issue already exists, do not create another one.
   - Return the existing issue instead.

9. Create the issue or issues using the configured GitHub MCP server.

10. Return links to all created or matching existing issues.

## Issue Fields

### Type

Choose exactly one:

- Business requirement
- Technical feature
- Technical improvement
- Refactoring / maintenance
- Other

Infer the type from the user's intent.

Do not change the substance of the feature to make it fit a type.

### Description

Describe the capability or behavior the system should provide.

The description answers:

> What are we adding or changing?

Preserve the user's intended meaning.

Do not add behavior that was not stated or clearly implied.

### Motivation

Explain why the feature or requirement is needed and what user, business, or technical need it addresses.

The motivation answers:

> Why should this capability exist?

This does not require an existing problem or behavior to change. A new capability may be motivated simply by a new user, business, or technical need.

Use only information stated or clearly implied by the user.

### Requirements

List rules, constraints, or specific conditions that the feature must satisfy.

The requirements answer:

> What rules must this capability follow?

Examples include:

- required inputs
- optional inputs
- default values
- validation rules
- conditional behavior
- business rules
- technical constraints explicitly requested by the user

Do not repeat the general feature description as a requirement.

Do not invent implementation requirements.

### Acceptance criteria

Express observable conditions that must be true for the issue to be considered complete.

The acceptance criteria answer:

> How do we know the feature is done?

Use testable checklist items:

- [ ] Criterion

Acceptance criteria should verify the description and requirements without introducing new behavior.

Prefer independently verifiable criteria.

### Technical notes

Include implementation details, architectural considerations, relevant components, classes, files, or technical constraints only when supplied by the user or clearly relevant to what the user explicitly requested.

Do not add technical decisions discovered from the codebase unless they are necessary to identify a factual contradiction.

### Additional context

Include relevant examples, references, related issues, background information, or other context supplied by the user.

## Rules

- Do not invent requirements.
- Do not invent missing motivation.
- Do not silently correct incorrect assumptions about the existing code.
- Do not turn observations from the codebase into new feature requirements.
- Use the codebase only to validate factual claims made by the user.
- Preserve the user's intent and terminology when they are factually valid.
- Infer information only when it follows clearly from the user's prompt.
- Do not create an issue when required information is missing.
- Do not create an issue whose premise contradicts the current codebase.
- Do not modify source code.
