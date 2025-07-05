Select * From "Blocks"
	ORDER BY id ASC LIMIT 100
	
Select * From "Floors"
	ORDER BY id ASC LIMIT 100
	
Select * From "HouseMapping"
	ORDER BY id ASC LIMIT 100
	
Select * from "Houses"
	ORDER BY id ASC LIMIT 100
	
Select * from "Roles"
	ORDER BY id ASC LIMIT 100
	
Select * from "UserOtp"
	ORDER BY id ASC LIMIT 100
	
Select * from "Users"
	ORDER BY id ASC LIMIT 100
	
Select * from "VehicleType"
	ORDER BY id ASC LIMIT 100
	
Select * from "Vehicles"
	ORDER BY id ASC LIMIT 100
	
Select * from "VisitPurpose"
	ORDER BY id ASC LIMIT 100
	
Select * from "VisitorStatus"
	ORDER BY id ASC LIMIT 100
	
Select * from "Visitors"
	ORDER BY id ASC LIMIT 100
	
Select * from "Notifications"
	ORDER BY id ASC LIMIT 100
	
Select * from "AudienceGroupTypes"
	ORDER BY id ASC LIMIT 100
	
Select * from "AudienceGroupMembers"
	ORDER BY id ASC LIMIT 100
	
Select * from "AudienceGroups"
	ORDER BY id ASC LIMIT 100

------------------------------------------------------------------------Update Table-------------------------------------------------------------------------------------------------------

Update "Visitors"
set is_approved = null
where is_approved is not null

	
update "Notifications"
	set read_at = null
	where read_at is not null

	
update "Visitors"
	set status_id = 4
	where status_id = 1 and created_at <= Current_date
	
update "Visitors"
	set status_id = 2
	where check_in_time is not null
	



------------------------------------------------------------------------Insert into Table-------------------------------------------------------------------------------------------------------

INSERT INTO public."AudienceGroups"(
	group_type_id, reference_id, group_name, description, created_by, updated_by)
	VALUES 
	(2, 2, 'Owner', 'Society Owners', 1, 1),
	(2, 3, 'Tenant', 'Society Tenant', 1, 1),
	(2, 4, 'Security', 'Society Security', 1, 1),
	(2, 5, 'Committee', 'Society Committee Members', 1, 1),
	(3, 1, 'A Block', 'A Block', 1, 1),
	(3, 2, 'B Block', 'B Block', 1, 1),
	(3, 3, 'C Block', 'C Block', 1, 1),
	(3, 4, 'D Block', 'D Block', 1, 1),
	(4, 1, 'First Floor', '1st Floor', 1, 1),
	(4, 2, 'Second Floor', '2nd Floor', 1, 1),
	(4, 3, 'Third Floor', '3rd Floor', 1, 1),
	(4, 4, 'Fourth Floor', '4th Floor', 1, 1),
	(4, 5, 'Fifth Floor', '5th Floor', 1, 1)


	
INSERT INTO "AudienceGroupTypes"
	(name) values
	('All'),
	('Role'),
	('Block'),
	('Floor'),
	('House'),
	('Custom')


	
INSERT INTO "NoticeCategories"
	(name) values
('General'),
('Emergency'),
('Repairs/Services'),
('Event'),
('Meeting'),
('Security Alert'),
('Billing/Payment'),
('Facilities Update'),
('Others');

insert into "Notifications"
	(receiver_id, message)
	values
	(1, 'Hello'),
	(10, 'Hey'),
	(20, 'Hiii')
	
	INSERT INTO "UserHouseMapping"(
	user_id, house_mapping_id, created_by)
	VALUES 
	(1, 1 ,1),
	(2, 2 ,1),
	(3, 3 ,1),
	(4, 4 ,1),
	(5, 5 ,1),
	(6, 6 ,1)


INSERT INTO "Visitors"(
	house_mapping_id, name, phone, visit_purpose_id, no_of_visitors, check_in_time, created_by, updated_by)
	VALUES 
	(10, 'Anisha', '9874563210', 1, 1, current_timestamp, 20, 1, 1),
	(10, 'Nisha', '7487465921', 2, 3, current_timestamp, 20, 1, 1),
	(10, 'Priya', '9654103782', 3, 2, current_timestamp, 20, 1, 1)

INSERT INTO "Visitors"(
	house_mapping_id, name, phone, visit_purpose_id, no_of_visitors, check_in_time, created_by, updated_by)
	VALUES 
	(10, 'Krish', '8745210364', 1, 1, current_timestamp, 20, 20)
	

INSERT INTO "Users"(
	role_id, email, password, name, phone, created_by, updated_by)
	VALUES 
	(3, 'krish@gmail.com', '$2a$11$xJn/LP132sZOTQRlr48QQO1AqKW.oSLvlrpLO4xV4z989xStf38gu', 'Krish', '9874563210', 1, 1),
	(4, 'security@gmail.com', '$2a$11$xJn/LP132sZOTQRlr48QQO1AqKW.oSLvlrpLO4xV4z989xStf38gu', 'Bahadur', '99899877577', 1, 1)
	
INSERT INTO "Blocks"(
	block_number, name, no_of_floor, created_by, updated_by)
	VALUES 
	(1, 'A', 4, 1, 1),
	(1, 'B', 4, 1, 1),
	(1, 'C', 4, 1, 1),
	(1, 'D', 4, 1, 1)

INSERT INTO "Floors"(
	floor_number, name, no_of_house, created_by, updated_by)
	VALUES 
	(1, 'First', 4, 1, 1),
	(1, 'Second', 4, 1, 1),
	(1, 'Third', 4, 1, 1),
	(1, 'Fourth', 4, 1, 1)

INSERT INTO "Houses"(
	house_number, name, created_by, updated_by)
	VALUES 
	(1, '101', 1, 1),
	(2, '102', 1, 1),
	(3, '103', 1, 1),
	(4, '104', 1, 1)

INSERT INTO "HouseMapping"(
	block_id, floor_id, house_id, created_by, updated_by)
	VALUES 
	(1, 1, 1, 1, 1),
	(1, 1, 2, 1, 1),
	(1, 1, 3, 1, 1),
	(1, 1, 4, 1, 1),
	(1, 2, 1, 1, 1),
	(1, 2, 2, 1, 1),
	(1, 2, 3, 1, 1),
	(1, 2, 4, 1, 1),
	(1, 3, 1, 1, 1),
	(1, 3, 2, 1, 1),
	(1, 3, 3, 1, 1),
	(1, 3, 4, 1, 1),
	(1, 4, 1, 1, 1),
	(1, 4, 2, 1, 1),
	(1, 4, 3, 1, 1),
	(1, 4, 4, 1, 1),

	(2, 1, 1, 1, 1),
	(2, 1, 2, 1, 1),
	(2, 1, 3, 1, 1),
	(2, 1, 4, 1, 1),
	(2, 2, 1, 1, 1),
	(2, 2, 2, 1, 1),
	(2, 2, 3, 1, 1),
	(2, 2, 4, 1, 1),
	(2, 3, 1, 1, 1),
	(2, 3, 2, 1, 1),
	(2, 3, 3, 1, 1),
	(2, 3, 4, 1, 1),
	(2, 4, 1, 1, 1),
	(2, 4, 2, 1, 1),
	(2, 4, 3, 1, 1),
	(2, 4, 4, 1, 1),

	(3, 1, 1, 1, 1),
	(3, 1, 2, 1, 1),
	(3, 1, 3, 1, 1),
	(3, 1, 4, 1, 1),
	(3, 2, 1, 1, 1),
	(3, 2, 2, 1, 1),
	(3, 2, 3, 1, 1),
	(3, 2, 4, 1, 1),
	(3, 3, 1, 1, 1),
	(3, 3, 2, 1, 1),
	(3, 3, 3, 1, 1),
	(3, 3, 4, 1, 1),
	(3, 4, 1, 1, 1),
	(3, 4, 2, 1, 1),
	(3, 4, 3, 1, 1),
	(3, 4, 4, 1, 1),

	(4, 1, 1, 1, 1),
	(4, 1, 2, 1, 1),
	(4, 1, 3, 1, 1),
	(4, 1, 4, 1, 1),
	(4, 2, 1, 1, 1),
	(4, 2, 2, 1, 1),
	(4, 2, 3, 1, 1),
	(4, 2, 4, 1, 1),
	(4, 3, 1, 1, 1),
	(4, 3, 2, 1, 1),
	(4, 3, 3, 1, 1),
	(4, 3, 4, 1, 1),
	(4, 4, 1, 1, 1),
	(4, 4, 2, 1, 1),
	(4, 4, 3, 1, 1),
	(4, 4, 4, 1, 1)


------------------------------------------------------------------------Create Table-------------------------------------------------------------------------------------------------------

-- Blocks
CREATE TABLE "Blocks"(
	id serial primary key,
	block_number int not null,
	name varchar(100) NOT NULL,
	no_of_floor int NOT NULL,
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)

	
-- Floors
CREATE TABLE "Floors"(
	id serial primary key,
	floor_number int not null,
	name varchar(100) not null,
	no_of_house int not null,
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)

	
-- House Mapping
create table "HouseMapping"(
	id serial primary key,
	block_id int not null references "Blocks"(id),
	floor_id int not null references "Floors"(id),
	house_id int not null references "Houses"(id),
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int,
	owner_id int references "Users"(id),
	tenant_id int references "Users"(id)
)

	
-- Houses
create table "Houses"(
	id serial primary key,
	house_number int not null,
	name varchar(100) not null, 
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)


-- Notice Attachments
CREATE TABLE IF NOT EXISTS "NoticeAttachments"
(
    id serial primary key,
    notice_id int not null references "Notices"(id),
	name varchar not null,
	path int not null,
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)

	
-- Notice Categories - Billing, Maintenance
CREATE TABLE IF NOT EXISTS "NoticeCategories"
(
    id serial primary key,
	name varchar(100) not null
)


	-- Notices
CREATE TABLE IF NOT EXISTS "Notices"
(
    id serial primary key,
	title varchar(100) not null,
	description varchar not null,
	notice_category_id int not null references "NoticeCategories"(id),
	target_audience_id int not null references "NoticeTargetAudiences"(id),
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)
	

-- Notification Categories
CREATE TABLE IF NOT EXISTS "NotificationCategories"
(
    id serial primary key,
	name varchar(100) not null
)


-- Notifications
CREATE TABLE IF NOT EXISTS "Notifications"
(
    id serial primary key,
    sender_id int references "Users"(id),
    receiver_id int NOT NULL references "Users"(id),
    message character varying(500) NOT NULL,
    read_time timestamp,
    created_at timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
)

	
-- Roles
CREATE TABLE "Roles"(
	id serial primary key,
	name varchar(255) NOT NULL,
	description varchar(500)
)
	
	
-- Users
CREATE TABLE "Users"(
	id serial primary key NOT NULL,
	role_id	int	NOT NULL  references "Roles"(id),
	email	varchar	NOT NULL,
	password varchar(255)	NOT NULL,
	name varchar(50) NOT NULL,
	profile_img	varchar,
	phone varchar,
	is_active	Boolean	NOT NULL default true,
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL,
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL,
	deleted_at timestamp,
	deleted_by int,
	is_approved boolean not null default false
)


-- Visitor Purpose
CREATE TABLE IF NOT EXISTS "VisitorStatus"
(
    id serial primary key,
	name varchar(100) not null
)


-- Visitor Feedbacks
CREATE TABLE IF NOT EXISTS "VisitorFeedbacks"
(
    id serial primary key,
	visitor_id int NOT NULL references "Visitors"(id)
    rating int,
    feedback varchar(500)
)


-- Visitor Status
CREATE TABLE IF NOT EXISTS "VisitorStatus"
(
    id serial primary key,
	name varchar(100) not null
)


-- Visitors
CREATE TABLE IF NOT EXISTS "Visitors"
(
    id serial primary key,
    house_mapping_id int NOT NULL references "HouseMapping"(id),
    name varchar(100) NOT NULL,
    phone varchar(15) NOT NULL,
    visit_purpose_id int NOT NULL references "VisitPurpose"(id),
    no_of_visitors int NOT NULL,
    check_in_time timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    check_out_time timestamp without time zone,
    vehicle_no character varying(15),
    is_approved boolean,
    created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int,
	target_entity_id int references "NotificationCategory",
	status_id int references "VisitorStatus"(id)
)


-- Notice Target Audiences Group Types - All, Role, Block, Custom
CREATE TABLE IF NOT EXISTS "AudienceGroupTypes"
(
    id serial primary key,
	name varchar(100) not null
)

-- Notice Target Custom Group Audience
CREATE TABLE IF NOT EXISTS "AudienceGroups"
(
    id serial primary key,
	group_name varchar(100) not null,
	description varchar(500) not null,
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	updated_at	timestamp NOT NULL default CURRENT_TIMESTAMP,
	updated_by	int NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int	
)

-- Notice Target Audience - Custom Group Members
CREATE TABLE IF NOT EXISTS "AudienceGroupMembers"
(
    id serial primary key,
	audience_group_id int not null references "AudienceGroups"(id) ON DELETE CASCADE,
	member_id int not null references "Users"(id) ON DELETE CASCADE,
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)

	
-- Notice & Target Audiences Mapping
CREATE TABLE IF NOT EXISTS "NoticeAudienceMapping"
(
    id serial primary key,
	notice_id int references "Notices"(id) ON DELETE CASCADE,
	group_type_id int not null references "AudienceGroupTypes"(id),		--Role, Block, Floor, Custom Group
	reference_id int,													--Primary Key eg. roleId, blockId, audience_group_id
	created_at	timestamp	NOT NULL default CURRENT_TIMESTAMP,
	created_by	int	NOT NULL references "Users"(id),
	deleted_at timestamp,
	deleted_by int
)

