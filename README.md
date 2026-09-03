# AutoGov9

A web based IT governance and e-maturity assessment platform for small and medium enterprises (SMEs), mapping staff self-assessments onto **COBIT 2019** and **ISO/IEC 38500** governance domains.

~MSc Software Engineering— School of Computer Science.

## Table of Contents

- [Business Case](#business-case)
- [Stakeholders & Actors](#stakeholders--actors)
- [Requirements (MoSCoW Traceability)](#requirements-moscow-traceability)
- [System Diagrams](#system-diagrams)
- [Data Model](#data-model)
- [Scoring Logic](#scoring-logic)
- [Design Rationale](#design-rationale)
- [Tech Stack](#tech-stack)
- [Known Limitations & Future Work](#known-limitations--future-work)

---

## Business Case

IT governance maturity assessments (against frameworks like COBIT and ISO 38500) are traditionally manual, consultant led, or locked behind enterprise GRC platforms priced for organisations with a dedicated compliance function:

| Tool | Positioning | Barrier for SMEs |
|---|---|---|
| OneTrust | Broad GRC platform (privacy, security, vendor risk) | Quote-based, per-module pricing; assumes a dedicated compliance team |
| vComply | Mid-market compliance workflow automation | Paid tier required before any framework mapping; scores compliance % rather than 0–5 capability |
| Hyperproof | Continuous evidence collection for SOC 2 / ISO 27001 | Per-seat pricing scales poorly for whole-workforce self-assessment |

Across all three, there is the same persistant gap: no lightweight option mapped to COBIT 2019 and ISO 38500 domains, at a cost an SME can absorb.

**AutoGov9's business objective:** let any SME self assess, score, and visualise IT governance and recieve an e-maturity score without external consultants or enterprise licensing costs.

## Stakeholders & Actors

| Actor | Role | Key Permissions |
|---|---|---|
| **Administrator** | Registers the company, manages assessment templates, oversees employees | Create/close assessments, view a full dashboard, generate unique company key |
| **Employee** | End user completing self-assessments | Join company via key, submit Likert scale responses, view own basic dashboard and messages from administrator |

Target end-user organisation: UK-based SMEs who currently either don't track IT e-maturity or do so manually via spreadsheets/consultants.

## Requirements (MoSCoW Traceability)

| ID | Requirement | Priority | Implemented |
|---|---|---|---|
| R1 | Admin registers company with personal details | Must | ✅ |
| R1.1 | Company name must be unique | Must | ✅ |
| R1.2 | Registering user is strictly assigned Admin role | Must | ✅ |
| R1.3 | Admin creates a company key on account creation | Must | ✅ |
| R2 | Employee registers with email/phone + personal details | Must | ✅ |
| R3 | All employee details stored in SQL Server | Must | ✅ |
| R4 | New user links to company via name + company key | Must | ✅ |
| R5 | Admin can view/modify all employee details | Should | ❌ |
| R6 | Admin can remove employee + record termination reason | Could | ❌ |
| R7 | Retain former-employee data for 6 months, then purge | Won't | ❌ |
| R8 | Employee can view a basic dashboard | Could | ✅ |
| R9 | Admin dashboard shows domain averages + overall E-Maturity score | Must | ✅ |
| R9.1 | Admin can clear an assessment period, resetting scores | Must | ✅ |
| R10 | Employee can view their own score in isolation | Could | ❌ |
| R11 | Employees cannot see other employees' personal details | Must | ✅ |
| R12 | Admin prepares a multiple-choice assessment questionnaire | Must | ✅ |
| R13 | Questionnaire is customisable | Should | ✅ |
| R14 | Assessment strictly enforces 20 questions, 4 per domain | Must | ✅ |
| R15 | Employees answer via Likert scale (not checkbox) | Should | ✅ |
| R16 | System displays source framework (COBIT/ISO) per question | Could | ✅ |
| R17 | Assessment results stored in SQL Server | Should | ✅ |
| R18 | Each user's question/answer pairs stored in SQL Server | Won't | ✅ |

### Financial Governance, FinOps & Longitudinal Tracking (v2.0 / In Progress)

| ID | Requirement | Priority | Implemented |
|---|---|---|---|
| R19 | Admin can assign simulated currency budgets to specific IT domains/projects | Must | 🟡 (In Dev) |
| R20 | System calculates domain financial risk exposure based on Likert maturity gaps | Should | 🟡 (In Dev) |
| R21 | System captures timestamped score snapshots upon assessment period closure | Must | 🟡 (In Dev) |
| R22 | Admin dashboard renders historical maturity trend lines across previous assessment periods | Should | 🟡 (In Dev) |
| R23 | Admin dashboard includes FinOps view comparing IT budget vs. maturity score ROI | Should | 📋 (Planned) |
| R24 | Assessment questionnaire options include SOX (Sarbanes-Oxley) compliance checks | Could | 📋 (Planned) |
| R25 | System calculates period-over-period percentage delta (growth/regression) per domain | Should | 📋 (Planned) |



**Verification method:** requirements traceability testing — each requirement exercised through its real user journey (registration → company linking → assessment creation → submission → dashboard scoring) rather than isolated unit tests. No load/performance testing has been carried out.

## System Diagrams

**UML Class Diagram** — domain entities and service layer.
<img width="1297" height="757" alt="image" src="https://github.com/user-attachments/assets/02da5fa5-5c7f-424f-a66f-f5c111400eee" />




**Entity Relationship Diagram** 
<img width="822" height="607" alt="image" src="https://github.com/user-attachments/assets/1ed907bc-56fc-4034-9b56-35217a51725b" />




**User Journey / Process Flow:**

```
Admin registers company → generates company key
        ↓
Employee registers → joins company via key
        ↓
Admin builds assessment template (20 Qs, 4 per domain, COBIT/ISO sourced)
        ↓
Employees complete assessment (1–5 Likert + optional comment)
        ↓
Admin closes assessment period
        ↓
System aggregates: per-submission domain average → cross-org domain average → overall maturity score
        ↓
Admin dashboard displays domain breakdown + overall E-Maturity score
```

## Data Model

- **Company** — 1:N with Users; stores name, address, and a unique company key for employee sign-up.
- **User** (abstract) → **Admin**, **Employee** — shared fields (name, email, password hash, status); Employee adds termination date/reason.
- **AssessmentTemplate** — owned by an Admin; contains a fixed list of 20 Questions (via a `JuncQuestions` junction table, enabling reuse across templates — a Many-to-Many relationship).
- **Question** — text, Framework (COBIT2019 / ISO38500), Domain.
- **AssessmentSubmission** — one per employee per completed assessment.
- **LikertAnswer** — one row per answered question, holding the 1–5 score and optional comment.

## Scoring Logic

Two-stage SQL aggregation, chosen so individual submissions remain independently meaningful rather than flattening all answers into one average:

1. **Stage 1 — per-submission, per-domain average:** average Likert score a single employee gave within a single domain, for a single submission.
2. **Stage 2 — cross-organisation domain average:** average those results across all submissions to produce one score per domain, company-wide.

**Final maturity score** = average of the domain averages (equal weight per domain), rounded to 2 decimal places.

## Design Rationale

The backend uses a **coupled 3-tier architecture** (Domain / Services / Data) with Blazor Server components calling services directly — no separate REST API/controller layer. 

Data acsess is performed using very SQL heavy dapper operations.

| | Pros | Cons |
|---|---|---|
| Coupled architecture | Less boilerplate, no duplicate API/UI models, faster feature delivery, page-focused encapsulation | Tightly bound to Blazor UI — harder to reuse backend for mobile/React/Angular; more cumbersome integration testing |

Chosen deliberately for project scope and timeline; flagged as a trade-off to revisit if the system were extended to other frontends.

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Server (C#, Razor components, SignalR) |
| Backend | C#, 3-tier architecture (Models / Services / Data) |
| Data access | Dapper (async), raw SQL |
| Database | Microsoft SQL Server (SSMS 2022) |

## Known Limitations & Future Work

- Currently working on adding a maturity tracking feature, which should track and display scores overtime. 
- Fixed 4-domain / 20-question structure is restrictive for enterprise-scale deployment.
- Only COBIT 2019 and ISO 38500 supported; NIST CSF extension would require re-validating the framework-mapping approach.
- No automated gap-analysis / remediation roadmap output yet (a core capability of commercial competitors like OneTrust).
- No performance/load testing against high-concurrency submissions across many companies.
- Employee management (view/edit/terminate) and personalised per-employee score views (R5, R6, R10) not yet implemented.
- Detailed Comments regarding employee termination, for example due to misconduct, could pose some meaningful insights if presented alongside the maturity tracking.
- Recommended next step: small-scale trial with a real SME to gather qualitative feedback on assessment UX.
