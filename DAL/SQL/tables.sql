-- documents table
CREATE TABLE IF NOT EXISTS documents(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	created TIMESTAMP DEFAULT NOW(),
	updated TIMESTAMP DEFAULT NOW(),
	name TEXT NULL,
	contenttype TEXT NULL,
	description TEXT NULL,
	version INTEGER NOT NULL DEFAULT 0,
	filepath TEXT NULL,
	filename TEXT NULL,
	filesize BIGINT NULL
);

-- products table
CREATE TABLE IF NOT EXISTS products(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	created TIMESTAMP DEFAULT NOW(),
	updated TIMESTAMP DEFAULT NOW(),
	name TEXT NOT NULL,
	description TEXT NULL,
	shortdescription TEXT NULL,
	price INTEGER NOT NULL DEFAULT 0,
	tags TEXT NULL
);

-- emailtemplates table
CREATE TABLE IF NOT EXISTS emailtemplates(
   id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
   created TIMESTAMP DEFAULT NOW(),
   updated TIMESTAMP DEFAULT NOW(),	
   name TEXT NOT NULL,
   subject TEXT NOT NULL,
   description TEXT NULL,
   version INTEGER NOT NULL DEFAULT 0,
   content TEXT NULL,
   key TEXT NOT NULL UNIQUE
);

-- profiledata table
CREATE TABLE IF NOT EXISTS profiledata(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	createdat TIMESTAMP DEFAULT NOW(),
	updatedat TIMESTAMP DEFAULT NOW(),	
	userid UUID NOT NULL UNIQUE,
	fullname TEXT NULL,
	phonenumber TEXT NULL,
	position TEXT NULL,
	email TEXT NULL,
	onlinecardurl TEXT NULL,
	profilepictureurl TEXT NULL
);

-- emailsettings table
CREATE TABLE IF NOT EXISTS emailsettings(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	emailtype TEXT NOT NULL,
	templateid UUID NULL
);

-- emaillog table
CREATE TABLE IF NOT EXISTS emaillog(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	createdat TIMESTAMP DEFAULT NOW(),
	updatedat TIMESTAMP DEFAULT NOW(),	
	userid UUID NULL,
	emailtypeid TEXT NULL,
	fromemail TEXT NULL,
	toemail TEXT NULL,
	templateid UUID NULL,
	status TEXT NULL
);

-- wikientity
CREATE TABLE IF NOT EXISTS wikis (
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	createdat TIMESTAMP DEFAULT NOW(),
	updatedat TIMESTAMP DEFAULT NOW(),
	title TEXT NOT NULL,
	markdown TEXT NULL,
	parentid UUID NULL,
	sortorder INT NOT NULL DEFAULT 0,
	isactive BOOLEAN NOT NULL DEFAULT TRUE
);

-- seed data
INSERT INTO users(id, email, passwordhash, created, userrole, lastlogin) VALUES ('ddf89a16-f57d-4f02-b5bb-42232dff11b9', 'info@median.dk', 'DBT3trJHhTmyo1E+xar0qQ==.GRVh7wFtw9JrOoVodikt6PczWuM+ci+xNr9mdEaE+gs=', now(), 'Admin', now());