**Standalone WinForms Data Entry & Reporting – PRD**

---

## 1. Purpose & Scope

A self‑contained Windows desktop application for managing customer and order data with built‑in reporting and exports. All data persists locally in SQLite; no external services required.

**In scope:**

- Customer and Order data entry (forms, validation, lookup)
- Local persistence via EF Core + SQLite (`data.db`)
- Data visualization (grids & charts) with ComponentOne controls
- Printable reports and file exports (PDF, Excel)
- User preferences (theme, default paths)

**Out of scope:**

- Web APIs or network sync
- Real‑time messaging/event streams

---

## 2. Technology Stack

- **.NET 9 WinForms**
- **ComponentOne WinForms Edition:**

  - **C1FlexGrid**: sortable, filterable, groupable grid
  - **C1Chart**: dynamic charts (bar, line)
  - **C1Report**: report designer and PDF/print export

- **EF Core 9.0.6** + **SQLite Provider**
- **MSTest/NUnit** for unit tests
- **WinAppDriver/FlaUI** for UI automation

---

## 3. Architecture Overview

```text
┌─────────────────────────────┐
│ WinForms Data App           │
│ • WinForms UI               │
│ • C1FlexGrid / C1Chart      │
│ • C1Report                  │
│ • EF Core → SQLite (data.db)│
│ • User Settings (.config)   │
└─────────────────────────────┘
```

- Single executable hosts UI, data access, and reporting
- EF Core `AppDbContext` applies migrations on startup
- `app.config` stores SQLite connection and user settings

---

## 4. Feature Implementation Details

### 4.1 Customer Management

- **UI**: `CustomerForm` with TextBoxes for Name, Email, Phone, Address
- **Validation**:

  - Required fields: `ErrorProvider` on leave event
  - Email regex validation
  - Phone number mask with `MaskedTextBox`
  - Summary of errors in a `Panel` at top

- **Data Access**:

  - `CustomerRepository` using EF Core CRUD
  - `SaveChanges()` wrapped in transaction

- **UI-Data Binding**:

  - Two-way binding: `BindingSource` for grid and form fields
  - On save/update, refresh `BindingSource.List`

### 4.2 Order Management

- **UI**: `OrderForm` with:

  - Dropdown (`ComboBox`) bound to `Customers` for selection
  - NumericUpDown for Quantity with min/max
  - DateTimePicker for OrderDate (no future dates)

- **Validation**:

  - Quantity > 0 and <= 1000
  - Date within last year to today

- **Data Access**:

  - `OrderRepository` methods: `Add`, `Update`, `Delete`
  - Soft delete flag (`IsDeleted`) for deletion

- **Business Rules**:

  - Prevent order creation if no customers exist
  - Display warning if stock < threshold (stub for future logic)

### 4.3 Grid & Chart Dashboard

- **Grid**:

  - Configure `C1FlexGrid`: `AllowSorting`, `AllowFiltering`, `GroupBy` on columns Customer, Month
  - Page size and virtual mode for >1000 rows

- **Charts**:

  - `C1Chart` bound to LINQ query: orders per month, order counts per customer
  - Tooltips show data point values

- **Refresh Mechanism**:

  - Central `DataRefreshService` raises event on data change
  - Dashboard subscribes and calls `ReloadData()`

### 4.4 Reporting & Export

- **C1Report Templates**:

  - Invoice report: customer details, order line items, totals
  - Summary report: date-filtered order summaries

- **Export Functions**:

  - PDF: `report.Render()` → `report.Export(..., FormatEnum.PDF)`
  - Excel: `C1FlexGrid.SaveExcel()` with column visibility settings
  - Prompt user with `SaveFileDialog`, remember last path in settings

### 4.5 User Preferences

- **SettingsForm**:

  - Theme selection: light/dark toggles WinForms colors via `Application.SetHighDpiMode`
  - Default export folder: browse dialog

- **Persistence**:

  - Use `UserScopedSettings` (`Properties.Settings.Default`)
  - Save on form close, load on startup

### 4.6 Additional Essential Features

#### 4.6.1 Logging & Error Handling

- **Global Exception Handler**: catch unhandled exceptions via `Application.ThreadException` and `AppDomain.CurrentDomain.UnhandledException` events
- **Logging**: write structured logs to file using `Serilog` or built-in `TraceSource`; include timestamps, stack traces
- **Error Dialog**: friendly error dialogs prompting to report issues; auto-save current form data

#### 4.6.2 Data Import

- **CSV/Excel Import**: allow users to import customer/order lists from `.csv` or `.xlsx`
- **Mapping UI**: simple mapping wizard to match columns to entity properties
- **Validation & Preview**: show preview rows and highlight errors before commit
- **Bulk Insert**: perform in transaction with rollback on failure

#### 4.6.3 Backup & Restore

- **Backup**: user-triggered export of `data.db` to timestamped `.bak` file
- **Restore**: option to select a backup file and replace current database (with confirmation)
- **Auto-Backup Option**: scheduled nightly backup via Windows Task Scheduler integration stub

#### 4.6.4 Help & About

- **Help Menu**: link to local `README.pdf` or online documentation
- **About Dialog**: display application version, author, license, and license key entry for future commercial use

#### 4.6.5 Auto-Update Stub

- **Update Check**: menu option to check for new versions via HTTP endpoint (URL configurable)
- **Download & Install**: download installer and launch it; prompt user to restart application

---

## 5. Data Flow & Sequence

1. **Startup**: `Program.Main` → load settings → `DbInitializer.Initialize()`
2. **Navigation**: Main menu selects Customer or Order form
3. **Entry**: Form validation → repo operation → commit → `DataRefreshService.Notify()`
4. **Dashboard**: Listens to refresh events → reload grid & charts
5. **Export/Print**: User action → generate report/grid export → file output or print
