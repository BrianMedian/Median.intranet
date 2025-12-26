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
)

-- products table
CREATE TABLE IF NOT EXISTS products(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	created TIMESTAMP DEFAULT NOW(),
	updated TIMESTAMP DEFAULT NOW(),
	name TEXT NOT NULL,
	description TEXT NULL,
	price INTEGER NOT NULL DEFAULT 0
)

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
)

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
)

-- emailsettings table
CREATE TABLE IF NOT EXISTS emailsettings(
	id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	emailtype TEXT NOT NULL,
	templateid UUID NULL
)