
INSERT INTO stakeholders."TourProblemReports"(
     "Id", "TourId", "Category", "Priority", "Description", "Time", "Status", "TouristId", "Comment", "SolvingDeadline", "Messages")
VALUES (-1, -1, 'Tehnički problem', 1, 'Problem sa internet konekcijom', '2024-10-02 11:30:00.246+02', 0, -21, 'aaa', null, '[]'::jsonb);

INSERT INTO stakeholders."TourProblemReports"(
     "Id", "TourId", "Category", "Priority", "Description", "Time", "Status", "TouristId", "Comment", "SolvingDeadline", "Messages")
VALUES (-2, -1, 'Bezbednosni problem', 1, 'Nedostupnost vodiča', '2024-10-02 11:30:00.246+02', 4, -21, 'aaa', '2025-10-05 11:30:00.246+02', '[]'::jsonb);

INSERT INTO stakeholders."TourProblemReports"(
     "Id", "TourId", "Category", "Priority", "Description", "Time", "Status", "TouristId", "Comment", "SolvingDeadline", "Messages")
VALUES (-3, -2, 'Oprema', 0, 'Oprema nije u dobrom stanju', '2024-10-03 09:15:00.246+02', 0, -21, 'aaa',  '2025-12-05 09:15:00.246+02', '[]'::jsonb);
