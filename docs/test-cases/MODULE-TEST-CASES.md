# WMS Module Test Cases

## Auth
| ID | Test Case | Expected |
|----|-----------|----------|
| AUTH-01 | Login with valid credentials | JWT token returned |
| AUTH-02 | Login with invalid password | 401 Unauthorized |
| AUTH-03 | Access protected route without token | 401 |

## Employees
| ID | Test Case | Expected |
|----|-----------|----------|
| EMP-01 | Create employee age < 18 | 400 Bad Request |
| EMP-02 | Search by name | Filtered results |
| EMP-03 | Update employee department | Record updated, audit logged |

## Attendance
| ID | Test Case | Expected |
|----|-----------|----------|
| ATT-01 | Check-in when not checked in | Attendance record created |
| ATT-02 | Double check-in same day | 400 error |
| ATT-03 | Download timesheet PDF | PDF file returned |

## Leaves
| ID | Test Case | Expected |
|----|-----------|----------|
| LVE-01 | Apply leave with ToDate < FromDate | 400 error |
| LVE-02 | Manager approves pending leave | Status = Approved |
| LVE-03 | Cancel pending leave | Record removed |

## Projects
| ID | Test Case | Expected |
|----|-----------|----------|
| PRJ-01 | Assign employee to project | Allocation Pending |
| PRJ-02 | Manager approves allocation | ApprovalStatus = Approved |
| PRJ-03 | Cancel allocation | Status inactive |

## Departments
| ID | Test Case | Expected |
|----|-----------|----------|
| DEP-01 | Create department | Record created |
| DEP-02 | Delete department | Record removed |

## Announcements
| ID | Test Case | Expected |
|----|-----------|----------|
| ANN-01 | Admin creates announcement | Visible on dashboard |
| ANN-02 | Deactivate announcement | Hidden from active list |

## Dashboard
| ID | Test Case | Expected |
|----|-----------|----------|
| DSH-01 | Load dashboard KPIs | Counts and charts returned |
