# C# Polyglot Coding Rules

## General Practices
- **ALWAYS** use clear and descriptive variable names in English, as C# is most commonly written and maintained in English.
- **MANDATORY**: Maintain consistent code style and conventions across different languages used in the project to ensure readability and maintainability.
- Code documentation **MUST** be in English to support international teams and contributors.

## Handling Language Interoperability
- **REQUIRED**: Ensure that all external libraries used for cross-language functionality are compatible with .NET standards.
- **ALWAYS** encapsulate language-specific logic within boundary classes or namespaces.

## Error Handling
- **MANDATORY**: Handle all cross-language operation errors gracefully, ensuring the errors are logged and user-friendly messages are displayed.
- **ALWAYS** use try-catch blocks around code that calls into different programming languages.

## Security
- **NEVER** expose direct interfaces of one language to another without appropriate security checks.
- **MUST** validate all inputs when data crosses language boundaries to prevent injection attacks.

## Performance
- **MANDATORY**: Optimize inter-language calls to minimize performance overhead.
- **RECOMMENDED**: Profile and monitor performance when implementing cross-language functionality to identify bottlenecks.

## Testing
- Integration tests **MUST** cover all polyglot components to ensure they work seamlessly together.
- **REQUIRED**: Use mock objects and services to simulate interactions between different languages during unit testing.

## Continuous Integration
- **MANDATORY**: Set up CI pipelines to build and test across all targeted languages.
- Code reviews **SHOULD** focus on the integration points between different languages to catch potential issues early.