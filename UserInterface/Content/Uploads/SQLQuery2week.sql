declare @userData TABLE(YEAR  INT, WEEKNUMBER  INT, STARTDATE  DATETIME, ENDDATE DATETIME)

--create procedure proc1(@year int)
--as
--begin
	declare @startdate datetime
			, @enddate datetime
			, @ctr int
	
	SET @startdate = CAST(2017 AS VARCHAR)+ '/04/01'
	SET @enddate = CAST(2017 + 1 AS VARCHAR) + '/03/31'
	SET @ctr = 0
	WHILE @enddate >= @startdate
	BEGIN
		SET @ctr = @ctr + 1
		INSERT INTO @userData
		values(year(@startdate), @ctr, @startdate, @startdate + 6)
		SET @startdate = @startdate + 7
	END
	
--end

--exec proc1 2011

select * from @userData