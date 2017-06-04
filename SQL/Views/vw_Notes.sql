USE [sackfacedb]

go

create view [dbo].[vw_Notes]
as
select 
	n.Id
	,n.Uid
	,n.Score
	,n.DateCreated
	,n.Subject
	,n.Body
	,n.Latitude
	,n.Longitude
	,n.Radius
	,n.IsAnonymous
	,u.Username
	,u.Id [UserId]
	,u.uid [UserUid]
from notes n
	inner join users u on n.userid = u.id
where 1=1
	and n.IsDeleted = 0

go