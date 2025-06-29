# Copilot Instructions for WinForms Data Entry & Reporting App

## Project Overview

A standalone Windows desktop application for managing customer and order data with built-in reporting and exports. All data persists locally in SQLite with no external service dependencies.

**Tech Stack:**

- .NET 9 WinForms
- ComponentOne WinForms Edition (C1FlexGrid, C1Chart, C1Report)
- EF Core 9.0.6 with SQLite Provider
- MSTest/NUnit for unit testing
- WinAppDriver/FlaUI for UI automation

## Core Commands

### Build & Development

```bash
# Standard .NET commands expected
dotnet build
dotnet run
dotnet test
dotnet test --filter "TestClass=SpecificTestClass"  # Single test class
```

### Task Management (Taskmaster)

```bash
# Initialize project tasks
task-master init
task-master parse-prd prd.md

# Task workflow
task-master list                    # View all tasks
task-master next                    # Get next available task
task-master show <id>              # View task details
task-master expand --id=<id>       # Break down complex tasks
task-master set-status --id=<id> --status=done

# Research & analysis
task-master research "<query>"     # Get up-to-date technical info
task-master analyze-complexity     # Analyze task complexity
```

## Architecture

### High-Level Structure

```
┌─────────────────────────────┐
│ WinForms Data App           │
│ • WinForms UI               │
│ • C1FlexGrid / C1Chart      │
│ • C1Report                  │
│ • EF Core → SQLite (data.db)│
│ • User Settings (.config)   │
└─────────────────────────────┘
```

### Key Components

- **UI Layer**: WinForms with ComponentOne controls for grids, charts, and reports
- **Data Layer**: EF Core `AppDbContext` with SQLite provider
- **Business Logic**: Repository pattern for CRUD operations
- **Reporting**: C1Report for PDF/print exports, Excel export via C1FlexGrid
- **Configuration**: User preferences stored in `.config`, connection strings in `app.config`

### Data Models

- **Customer**: Name, Email, Phone, Address (with validation)
- **Order**: Customer reference, Quantity, OrderDate, soft delete support
- **Settings**: Theme preferences, default export paths

## Repository-Specific Style Rules

### Data Access Patterns

- Use Repository pattern for all CRUD operations
- Wrap `SaveChanges()` in transactions
- Implement soft delete with `IsDeleted` flag for orders
- Apply EF migrations on startup via `DbInitializer.Initialize()`

### UI Patterns

- Use `ErrorProvider` for field validation on leave events
- Implement two-way binding with `BindingSource` for grids and forms
- Use `DataRefreshService` for coordinated UI updates across forms
- Configure ComponentOne controls: `AllowSorting`, `AllowFiltering`, `GroupBy`

### Validation Rules

- **Email**: Regex validation required
- **Phone**: Use `MaskedTextBox` for formatting
- **Quantity**: NumericUpDown with min/max (1-1000)
- **Dates**: No future dates, within last year to today
- **Required Fields**: Show summary of errors in top panel

### Error Handling

- Global exception handlers via `Application.ThreadException` and `AppDomain.CurrentDomain.UnhandledException`
- Use `Serilog` or `TraceSource` for structured logging with timestamps
- Friendly error dialogs with auto-save current form data
- Transaction rollback on bulk operation failures

### Testing Patterns

- Unit tests with MSTest/NUnit for business logic
- UI automation tests with WinAppDriver/FlaUI
- Mock repositories for isolated testing
- Test data validation rules and business constraints

## Taskmaster Integration

This project uses [Taskmaster](https://github.com/sammcj/taskmaster) for AI-powered task management:

### Key Workflow Rules

- **Start with research**: Use `task-master research` before implementing tasks to get current best practices
- **Log implementation progress**: Use `task-master update-subtask` to document findings with timestamps
- **Break down complex tasks**: Use `task-master expand` for tasks with complexity score >7
- **Update affected tasks**: Use `task-master update` when implementation differs from plan
- **Mark dependencies**: Ensure prerequisite tasks are completed before starting dependent work

### Agent Guidelines from `.github/instructions/`

- Follow **dev_workflow.md** for Taskmaster development patterns
- Use MCP tools over CLI when available for better performance
- Generate individual markdown task files only when needed (`task-master generate`)
- Leverage AI research for up-to-date technical guidance beyond training cutoff
- Use tagged task lists for feature branches or experiments

### Configuration Management

- AI model settings in `.taskmaster/config.json` (managed via `task-master models`)
- API keys in `.env` (CLI) or `.vscode/mcp.json` (VS Code integration)
- Current models: Google Gemini 2.5 Pro for main/research, fallback configured

## Key Development Patterns

### Form Implementation

```csharp
// Standard form validation pattern
private void ValidateField_Leave(object sender, EventArgs e)
{
    errorProvider.SetError(control, validationMessage);
    UpdateErrorSummary();
}
```

### Repository Pattern

```csharp
// Transaction-wrapped operations
public async Task<Customer> SaveCustomerAsync(Customer customer)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    // ... save logic
    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}
```

### ComponentOne Grid Configuration

```csharp
// Standard grid setup
flexGrid.AllowSorting = true;
flexGrid.AllowFiltering = true;
flexGrid.AllowGroupBy = true;
// Use virtual mode for >1000 rows
```

## Important Notes

- **No Web APIs**: This is a standalone desktop application
- **Local SQLite**: All data persists in `data.db` file
- **ComponentOne License**: Commercial controls require proper licensing
- **Soft Deletes**: Orders use `IsDeleted` flag, not hard deletion
- **User Settings**: Store in `Properties.Settings.Default` for persistence
- **Export Formats**: Support PDF (via C1Report) and Excel (via C1FlexGrid)

For detailed task management workflows, see [dev_workflow.md](.github/instructions/dev_workflow.md) and [taskmaster.md](.github/instructions/taskmaster.md).
