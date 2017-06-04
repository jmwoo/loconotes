use sackfacedb

GO

DROP PROCEDURE [dbo].[sp_GetNearbyNotes]
GO

CREATE PROCEDURE [dbo].[sp_GetNearbyNotes]
	@applicationUserId int = null
	,@latitude decimal(9,6)
	,@longitude decimal(9,6)
	,@minLatitude decimal(9,6)
	,@maxLatitude decimal(9,6)
	,@minLongitude decimal(9,6)
	,@maxLongitude decimal(9,6)
	,@take int = 10

AS
BEGIN
	SET NOCOUNT ON;

select top (@take)
	n.*
	,v.value [MyVote]
from vw_Notes n
	left outer join Votes v on (1=1
		and @applicationUserId is not null
		and v.Userid = @applicationUserId
		and n.Id = v.NoteId
	)
where 1=1
	and (
		(n.Latitude between @minLatitude and @maxLatitude) 
		and (n.Longitude between @minLongitude and @maxLongitude)
	)
order by
	geography::Point(@latitude, @longitude, 4326).STDistance(geography::Point(n.Latitude, n.Longitude, 4326))
	,n.Id desc

END

GO

