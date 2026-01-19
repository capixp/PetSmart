# DesignPatterns – C# Architectural Patterns

This solution contains focused implementations of classic **software design patterns** using modern C# and .NET practices.

The goal is not only to show *how* patterns are implemented, but *why* they exist and *when* to use them.

---

## Solution Structure

DesignPatterns/
├─ DesignPatterns.sln
├─ BankFactory/
└─ ConsoleAppClient/


---

## Implemented Patterns

### Factory Pattern

**Problem addressed:**  
Avoid tight coupling between client code and concrete implementations.

**Solution:**  
Encapsulate object creation logic behind a factory abstraction.

**Benefits:**
- Adheres to the Open/Closed Principle
- Simplifies adding new implementations
- Improves testability
- Eliminates `new` from consuming code

---

## Project Responsibilities

### BankFactory
**Role:**  
Contains:
- Factory abstractions
- Concrete factory implementations
- Domain-specific products (e.g. tax calculators)

**Key concepts:**
- Factory pattern
- Interface-driven design
- Keyed service resolution
- Dependency Injection

---

### ConsoleAppClient
**Role:**  
Acts as the consumer of the factory.

**Responsibilities:**
- Requests abstractions, not concrete classes
- Demonstrates runtime resolution of implementations
- Shows how DI and factories work together

---

## Dependency Injection Strategy

- Factories and products are registered in the DI container
- Object creation is delegated to the container
- Consumers remain unaware of concrete implementations

This approach enforces **Inversion of Control** and aligns with modern .NET architecture.

---

## Design Principles Applied

- **Single Responsibility Principle**
- **Open / Closed Principle**
- **Dependency Inversion Principle**
- **Separation of Concerns**

---

## How to Run

1. Open `DesignPatterns.sln`
2. Set `ConsoleAppClient` as the startup project
3. Run the application
4. Observe behavior changes by switching factory keys

---

## Interview Notes

This solution is designed to demonstrate:
- Understanding of design patterns beyond syntax
- Practical use of Dependency Injection
- Clean and extensible architecture
- Real-world application of SOLID principles

---

## Author

Wagner Alvarado  
Software Developer
