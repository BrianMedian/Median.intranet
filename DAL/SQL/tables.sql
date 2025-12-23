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