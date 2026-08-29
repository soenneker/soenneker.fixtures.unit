[![](https://img.shields.io/nuget/v/Soenneker.Fixtures.Unit.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Fixtures.Unit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.fixtures.unit/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.fixtures.unit/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Fixtures.Unit.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Fixtures.Unit/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.fixtures.unit/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.fixtures.unit/actions/workflows/codeql.yml)

# Soenneker.Fixtures.Unit

A base xUnit fixture providing injectable log output, DI mechanisms like IServiceCollection and ServiceProvider, and AutoFaker/Faker for generating test data.

## Install

```bash
dotnet add package Soenneker.Fixtures.Unit
```

## What you get

- `IUnitFixture` — A base xUnit fixture providing injectable log output, DI mechanisms like IServiceCollection and ServiceProvider, and AutoFaker/Faker for generating test data.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IUnitFixture.ServiceProvider` | Gets or sets the service provider used to resolve dependencies. | Gets or sets the service provider used to resolve dependencies. |
| `IUnitFixture.Services` | Gets or sets the collection of service descriptors. | Gets or sets the collection of service descriptors. |
| `IUnitFixture.Faker` | Gets an instance of `Faker` used for generating fake data. | Gets an instance of `Faker` used for generating fake data. |
| `IUnitFixture.AutoFaker` | Gets an instance of `AutoFaker` used for generating auto-mocked fake data. | Gets an instance of `AutoFaker` used for generating auto-mocked fake data. |
