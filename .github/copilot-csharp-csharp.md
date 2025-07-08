# C# Coding Standards for AI Coding Assistant 'copilot'

## General Principles
- **MANDATORY**: Adhere to the latest C# language version to ensure code uses the most current features and improvements.
- **ALWAYS** write code that is clear, understandable, and maintainable.
- **NEVER** use obsolete or deprecated C# features unless absolutely necessary for maintaining legacy systems.

## Formatting and Style
- **MUST** follow Microsoft's C# coding conventions regarding naming, layout, and commenting.
  - Use PascalCase for class names and method names.
  - Use camelCase for local variables and method arguments.
  - **REQUIRED**: Place opening braces on a new line for classes, methods, and control statements.
- **ALWAYS** indent code blocks with four spaces, not tabs.
- **ALWAYS** use explicit typing over implicit var declarations, except in cases of obvious typing (e.g., instantiation).

## Error Handling
- **MANDATORY**: Handle all possible exceptions using try, catch, finally blocks where applicable.
- **NEVER** allow exceptions to go unhandled unless explicitly justified by the application logic.
- Exceptions **SHOULD** be logged with a detailed error message and, if applicable, stack trace.

## Security Practices
- **MUST** validate all inputs to prevent injection attacks and ensure data integrity.
- **REQUIRED**: Utilize secure methods and libraries for handling sensitive data such as passwords, API keys, and personal user information.
- **NEVER** log sensitive information.

## Performance
- **USE** async and await for asynchronous programming to improve application responsiveness and scalability.
- **RECOMMENDED**: Optimize LINQ queries by minimizing data fetching and processing operations.
- **SHOULD** prefer StringBuilder over string concatenation in loops or during extensive string manipulations.

## Testing
- **MANDATORY**: Write unit tests for all business logic to ensure code quality and catch regressions early.
- Unit tests **SHOULD** cover both positive and negative scenarios.
- **ALWAYS** use a consistent test naming convention that clearly describes the test purpose.

## Code Reviews and Collaboration
- **MUST** use pull requests for merging code into shared branches and repositories.
- **ALWAYS** perform thorough code reviews to catch issues early and share knowledge among team members.
- **REQUIRED**: Follow a defined Git workflow suitable for your team structure (e.g., Git flow, GitHub flow).

## Documentation
- **REQUIRED**: Document all public classes, methods, properties, and enums.
- **ALWAYS** update documentation to reflect changes in the logic or implementation.
- **SHOULD** include inline comments to clarify complex code blocks.

## Dependency Management
- **MANDATORY**: Keep external dependencies to a minimum to reduce potential security risks and lower the maintenance burden.
- **ALWAYS** audit dependencies for security vulnerabilities on a regular schedule.
- When new libraries are added, **SHOULD** assess their stability and support level.

## Continuous Integration/Continuous Deployment
- **MUST** have CI/CD pipelines in place to automate testing and deployment processes.
- Deployment scripts **SHOULD** be tested regularly to prevent deployment failures.

## Legacy Code and Migration
- **ALWAYS** aim to incrementally refactor legacy code as part of ongoing development.
- When dealing with legacy systems, **SHOULD** plan a clear migration path towards newer technologies or frameworks.

These rules are designed to ensure that the AI coding assistant 'copilot' and its users adhere to the best practices and modern standards prevalent in C# development as of 2025.