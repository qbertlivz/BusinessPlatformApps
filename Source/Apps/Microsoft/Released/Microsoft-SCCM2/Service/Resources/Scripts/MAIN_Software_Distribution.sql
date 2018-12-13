SET NOCOUNT ON;

SELECT Sum(numbersuccess)    AS success,
       Sum(numberinprogress) AS inprogress,
       Sum(numberunknown)    AS unknown,
       Sum(numbererrors)     AS error,
       Sum(numberother)      AS other
FROM   v_deploymentsummary
WHERE  featuretype = 1  