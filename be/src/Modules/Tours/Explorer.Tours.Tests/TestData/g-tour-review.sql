INSERT INTO tours."TourReview"(
"Id", "TourId", "Rating", "Comment", "TouristId", "VisitDate", "ReviewDate","CompletedPercentage", "Images")
VALUES (-1, -1, 5, 'Amazing tour, highly recommended!', -1, '2023-05-15', '2023-06-01', 60, ARRAY['img1.jpg', 'img2.jpg']);

INSERT INTO tours."TourReview"(
"Id", "TourId", "Rating", "Comment", "TouristId", "VisitDate", "ReviewDate", "CompletedPercentage", "Images")
VALUES (-2, -2, 4, 'Great experience, but a bit too long.', -2, '2023-07-12', '2023-07-20', 80, ARRAY['img3.jpg']);

INSERT INTO tours."TourReview"(
"Id", "TourId", "Rating", "Comment", "TouristId", "VisitDate", "ReviewDate", "CompletedPercentage", "Images")
VALUES (-3, -3, 3, 'It was okay, but I expected more.', -3, '2023-08-05', '2023-08-12', 75, ARRAY[]::text[]);