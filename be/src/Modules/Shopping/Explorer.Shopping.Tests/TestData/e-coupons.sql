INSERT INTO shopping."Coupons"(
	"Id", "Identifier", "Percentage", "ExpirationDate", "AuthorId", "ToursEligible", "CouponStatus")
VALUES 
	(-1, 'SPRING2024', 20, '2024-03-01', 1, ARRAY[1, 2, 3], 0);

INSERT INTO shopping."Coupons"(
	"Id", "Identifier", "Percentage", "ExpirationDate", "AuthorId", "ToursEligible", "CouponStatus")
VALUES 
	(-2, 'SUMMER50', 50, '2024-06-15', 2, ARRAY[4], 1);

INSERT INTO shopping."Coupons"(
	"Id", "Identifier", "Percentage", "ExpirationDate", "AuthorId", "ToursEligible", "CouponStatus")
VALUES 
	(-3, 'WINTER15', 15, '2024-12-31', 3, ARRAY[8, 9, 10], 0);
