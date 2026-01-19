# PetSmart – .NET Architecture & Design Patterns Repository

This repository contains multiple .NET solutions demonstrating **real-world application development** and **clean architectural practices**, along with **design pattern implementations** used to reinforce software engineering fundamentals.

The structure and decisions in this repository are intentionally aligned with **industry standards** and **technical interview expectations**.

---

## Repository Structure

PetSmart/
├─ ProductApp.sln
├─ ProductApp/
├─ ProductApp.Tests/
├─ DesignPatterns/
│ ├─ DesignPatterns.sln
│ ├─ BankFactory/
│ └─ ConsoleAppClient/
└─ .gitignore

## Solutions Overview

### ProductApp Solution
**Purpose:**  
Production-oriented application showcasing clean structure, separation of concerns, and testability.

**Key concepts demonstrated:**
- Layered architecture
- Dependency Injection
- Test-driven development (unit tests)
- Maintainable solution structure

**Projects:**
- `ProductApp` – Core application logic
- `ProductApp.Tests` – Automated tests

**How to run:**
1. Open `ProductApp.sln`
2. Restore NuGet packages
3. Run the application or execute tests

---

### DesignPatterns Solution
**Purpose:**  
Hands-on implementations of classic **design patterns** with emphasis on **why** and **when** to use them.

**Projects:**
- `BankFactory` – Factory Pattern implementation
- `ConsoleAppClient` – Consumer of the factory via dependency injection

**Patterns covered:**
- Factory Pattern
- Dependency Injection
- Open/Closed Principle
- Inversion of Control (IoC)

**How to run:**
1. Open `DesignPatterns/DesignPatterns.sln`
2. Set `ConsoleAppClient` as startup project
3. Run the solution

---

## Architecture & Design Philosophy

- Each solution is **self-contained**
- Learning material (DesignPatterns) is isolated from production code (ProductApp)
- Projects are grouped by **business responsibility**, not technical type
- Code prioritizes:
  - Readability
  - Testability
  - Extensibility

This layout reflects how modern .NET solutions are structured in professional environments.

---

## Why Multiple Solutions in One Repository?

- Clear separation between:
  - Production-ready code
  - Architectural experiments and learning
- Allows independent evolution of each solution
- Common approach in teams for internal tooling, spikes, or POCs

---

## Requirements

- .NET SDK (latest LTS recommended)
- Visual Studio 2022+ or compatible .NET IDE

---

## Notes for Reviewers / Interviewers

- The repository intentionally favors **clarity over complexity**
- Patterns are implemented using modern .NET DI features
- Code is structured to be easily extendable without modification

---

## Author

**Wagner Alvarado**  
Software Developer  
