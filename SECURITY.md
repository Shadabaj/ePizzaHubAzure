# Security Policy

## Reporting a vulnerability

Please do not publish credentials, production connection strings, payment secrets, or personal data in public issues. Use GitHub's private vulnerability reporting feature if enabled, or contact the repository owner privately.

## Configuration hygiene

- Keep real application settings and Azure Functions local settings out of Git.
- Use .NET user secrets, environment variables, or a managed secret store for developer and deployment configuration.
- Treat credentials that were previously committed to a public repository as exposed: rotate them at the issuing service.
- Removing a credential from the latest version does not remove it from Git history. Consider an approved history cleanup only after credentials have been rotated and all collaborators are informed.
