# C# Project Rules: Docker Integration

## General Docker Practices

- **MANDATORY**: All Dockerfiles **MUST** be located in the root of the project repository.
- **MANDATORY**: Docker images **MUST** be built using official .NET base images from Docker Hub unless a specific need dictates otherwise.

## Dockerfile Configuration

- **REQUIRED**: The base image in the Dockerfile **MUST** match the .NET version used in the project.
- **ALWAYS** use multi-stage builds to reduce the size of the final Docker image.
  - The `build-env` for compiling the application.
  - The `runtime-env` for the execution environment.
- **NEVER** leave API keys, secrets, or other sensitive data in the Docker image.
- **MANDATORY**: Docker build context **SHOULD** be kept as small as possible by properly setting `.dockerignore` files.
- Environment variables **SHOULD** be used for application configurations that vary between environments (e.g., Development, Testing, Production).

## Docker Compose Configurations

- **REQUIRED**: Use Docker Compose for managing multi-container Docker applications.
- **ALWAYS** define service dependencies explicitly in `docker-compose.yml`.
- **MANDATORY**: Version of Docker Compose file **MUST** align with the latest supported Docker Engine and Compose specifications as of 2025.

## CI/CD Integrations

- **REQUIRED**: Integrate Docker builds into your CI/CD pipeline.
- **MANDATORY**: Pull requests **MUST NOT** be merged unless the Docker image builds successfully.
- **ALWAYS** tag Docker images with both a unique build tag and the `latest` tag in CI pipelines for easier rollback and deployment.

## Security Best Practices

- **MANDATORY**: Use Docker’s built-in security features such as `--cap-drop=all` and `readonly` filesystem where appropriate.
- **MANDATORY**: Update the Docker images regularly to include the latest security patches for the base images and dependencies.
- **ALWAYS** scan Docker images for vulnerabilities during CI/CD before pushing to a registry.
- **REQUIRED**: Use private Docker registries for internal or sensitive projects.

## Performance Optimizations

- **MANDATORY**: Minimize the number of layers in Docker images where feasible by consolidating commands.
- Images **SHOULD** leverage Docker cache layers by ordering Dockerfile instructions from least to most frequently changed.
- **REQUIRED**: Performance critical settings, like CPU and memory limits, **MUST** be configured explicitly in Docker Compose configurations or Kubernetes deployment specs. 

## Versioning and Tagging

- **MANDATORY**: Docker images **MUST** be versioned appropriately using semantic versioning principles.
- **ALWAYS** use explicit version tags rather than relying on mutable tags like `latest` for production deployments.

By following these rules, the Docker integration for C# projects will adhere to modern standards and best practices as of 2025, ensuring efficiency, security, and maintainability.