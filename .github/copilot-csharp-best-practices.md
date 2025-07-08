# C# Coding Standards and Best Practices

## Code Style

- **MANDATORY**: Follow the .NET coding conventions as defined by Microsoft.
- **ALWAYS** use `camelCase` for local variables and method parameters.
- **ALWAYS** use `PascalCase` for method names, property names, and class names.
- **MANDATORY**: Ensure that braces are used consistently for all control structures. Example:
  ```csharp
  if (x == y)
  {
      Foo();
  }
  ```
- **ALWAYS** keep lines of code reasonably short; lines should not exceed 120 characters.

## Naming Conventions

- **MANDATORY**: Use meaningful and descriptive names for classes, methods, and variables.
- **NEVER** use abbreviated names unless they are well-known (e.g., `Id` for Identifier).
- **REQUIRED**: Prefix interfaces with the letter `I`.
- **ALWAYS** name namespaces starting with the company name followed by the project and any sub-folders aligning with the project structure.

## Error Handling

- **MANDATORY**: Use exceptions for abnormal or unexpected program behavior.
- **ALWAYS** provide helpful error messages when throwing exceptions.
- **NEVER** use exceptions for normal flow of control.
- `recommended`: Prefer `try/catch` over returning error codes.

## Documentation

- **REQUIRED**: Every public class and method **MUST** have XML documentation comments for IntelliSense and documentation tools.
- **MANDATORY**: Document all parameters, exceptions thrown, and return values.
- `recommended`: Regularly update documentation to reflect changes in the codebase.

## Security

- **MUST** validate all inputs to avoid SQL injections and other common security threats.
- **ALWAYS** use secure methods and classes provided by the .NET framework where available.
- `recommended`: Utilize Code Analysis (FxCop) to detect security flaws.

## Performance

- **MANDATORY**: Avoid unnecessary object creation within loops.
- `recommended`: Utilize lazy loading where appropriate.
- `recommended`: Employ caching mechanisms to improve responsiveness and performance.

## Source Control

- **MANDATORY**: Check in code frequently to avoid large sets of unshared changes.
- **REQUIRED**: Include meaningful commit messages with each check-in.
- **NEVER** check in commented-out code or unnecessary files.

## Testing

- **MANDATORY**: Write unit tests for all new methods and classes.
- **ALWAYS** run the full test suite before committing your code to the repository.
- **REQUIRED**: Aim for at least 80% code coverage in tests for all business logic classes.

## API Design

- **MUST** ensure that public APIs are intuitive and well-documented.
- **ALWAYS** use consistent parameter ordering across similar methods.
- `recommended`: Avoid breaking changes to public APIs.

## Versioning

- **MANDATORY**: Adhere to semantic versioning rules for all public software releases.
- **ALWAYS** increment version numbers based on the extent of changes:
  - **MAJOR** version when you make incompatible API changes,
  - **MINOR** version when you add functionality in a backward-compatible manner,
  - **PATCH** version when you make backward-compatible bug fixes.

## Dependability

- `recommended`: Rely on proven .NET libraries and frameworks rather than creating custom implementations.
- **NEVER** ignore deprecation notices without assessing impact.

This set of rules defines the essential guidelines that **MUST**, **SHOULD**, and **NEVER** be violated in any professional C# project as of 2025. These standards are in place to ensure reliability, security, and maintainability of code.