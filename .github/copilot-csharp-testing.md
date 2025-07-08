# C# Project Testing Guidelines

## General Principles

- **MANDATORY**: Use a recognized testing framework such as NUnit, xUnit, or MSTest for unit testing.
- **ALWAYS** ensure each test case is independent and can be run in any order.
- **REQUIRED**: Maintain a clean separation of test logic from production code.
- **MUST** configure Continuous Integration (CI) to run tests automatically upon code pushes or pull requests.

## Writing Tests

### Structure

- **MANDATORY**: Structure tests logically using [Arrange-Act-Assert (AAA)](https://www.typemock.com/unit-test-patterns-for-net) pattern.
- **MUST** name test methods clearly to reflect the behavior being tested.

### Assertions

- **MUST** use assertive statements that make the test purpose clear.
- **REQUIRED**: Employ fluent assertions for more readable tests.

### Code Coverage

- **MANDATORY**: Aim for at least 80% code coverage to ensure most of the code is scrutinized by tests.
- **RECOMMENDED**: Use tools such as Coverlet or Visual Studio Coverage tools to measure test coverage.

## Test Isolation

- **ALWAYS** use mocks and stubs to isolate the unit of work and avoid external dependencies in unit tests.
- **MANDATORY**: Employ frameworks like Moq, NSubstitute, or FakeItEasy for mocking.

## Test Data

- **MUST** handle test data carefully, ensuring no leakage between tests.
- **ALWAYS** prefer the use of in-memory databases like EF Core InMemory provider for integration testing.

## Performance

- **RECOMMENDED**: Regularly review and optimize the performance of test suites.
- **SHOULD** avoid long-running tests in the primary test suite to keep the CI process efficient.

## Security

- **NEVER** use real data in testing environments to avoid risks of sensitive data exposure.
- **ALWAYS** ensure tests validate input sanitation and adheres to security best practices.

## Documentation

- **REQUIRED**: Document the testing strategy and major choices in a TESTPLAN.md file.
- Tests **SHOULD** include inline comments where necessary to explain complex logic or decisions.

## Error Handling

- **MANDATORY**: Tests **MUST** assert appropriate error handling in the application code and never ignore exceptions unless explicitly testing exception handling scenarios.

## Maintenance

- **NEVER** allow failing tests to accumulate or remain unfixed.
- **SHOULD** regularly review and refactor tests to improve clarity and maintainability.