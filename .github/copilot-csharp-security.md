# Security Rules for C# Projects

## General Security Practices
- **MANDATORY** : Always validate and sanitize all user inputs to prevent SQL injections, cross-site scripting (XSS), and other injection attacks.
- **MUST** : Employ strong, up-to-date cryptographic practices for data encryption, signing, and hashing.
- **ALWAYS** : Ensure that error messages do not reveal details about the backend system, such as file paths or component versions.

## Authentication and Session Management
- **MUST** : Implement secure authentication mechanisms like OAuth, OpenID Connect, or SAML.
- **ALWAYS** : Use HTTPS and secure cookies (HttpOnly, Secure attributes) for session management.
- **MANDATORY** : Implement multi-factor authentication for accessing sensitive data or operations.

## Authorization
- **MUST** : Enforce the Principle of Least Privilege (PoLP) throughout the application.
- **ALWAYS** : Use role-based access control (RBAC) or attribute-based access control (ABAC) to manage user permissions.

## Data Protection
- **MANDATORY** : Encrypt sensitive data both at rest and in transit using industry-standard protocols and algorithms.
- **MUST** : Properly manage and rotate keys and secrets using a secure vault solutions like Azure Key Vault or AWS Secrets Manager.

## Code Security
- **NEVER** : Leave debug code or secrets (API keys, passwords) in the version control system.
- **MANDATORY** : Use static code analysis tools to automatically detect and rectify security vulnerabilities in the codebase.
- **ALWAYS** : Perform regular code reviews with a focus on security implications and vulnerabilities.

## External Dependencies
- **REQUIRED** : Regularly update and patch all third-party libraries and frameworks to protect against known vulnerabilities.
- **ALWAYS** : Vet external libraries for security vulnerabilities before including them in the project.

## Logging and Monitoring
- **ALWAYS** : Implement logging and monitoring to detect and respond to security breaches or irregular activity.
- **NEVER** : Log sensitive data like passwords or personal identification information.

## Network Security
- **MUST** : Protect all network communications with TLS.
- **ALWAYS** : Implement strong network isolation and segmentation strategies to limit the blast radius in case of a network intrusion.

## Incident Response
- **REQUIRED** : Have an incident response plan that includes immediate actions, reporting to stakeholders, and remedial steps.
- **MANDATORY** : Conduct regular security drills to ensure that all team members know their roles in case of a security incident.

## Compliance
- **ALWAYS** : Adhere to relevant industry security standards and regulations, such as GDPR, PCI DSS, or HIPAA, depending on the project scope.
- **MANDATORY** : Undergo regular security audits and compliance checks to maintain system integrity and trust.

By following these rules, C# projects can mitigate security risks and protect user data effectively.