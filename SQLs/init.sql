  -- Table: orders

-- DROP TABLE orders;

CREATE TABLE orders
(
  id serial NOT NULL,
  fkuserid integer,
  commoditytype integer,
  amount integer,
  price integer,
  ordertype character varying,
  orderdate timestamp without time zone,
  fkmatchingorderid integer,
  ismatchsearched boolean,
  CONSTRAINT orders_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE orders
  OWNER TO postgres;



  -- Table: registrations

-- DROP TABLE registrations;

CREATE TABLE registrations
(
  id serial NOT NULL,
  name character varying,
  ktpnumber character varying,
  phonenumber character varying,
  address character varying,
  isverified boolean,
  CONSTRAINT registrations_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE registrations
  OWNER TO postgres;


-- Table: users

-- DROP TABLE users;

CREATE TABLE users
(
  id serial NOT NULL,
  name character varying,
  ktpnumber character varying,
  phonenumber character varying,
  address character varying,
  longitude double precision,
  latitude double precision,
  CONSTRAINT users_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE users
  OWNER TO postgres;


CREATE OR REPLACE VIEW orders_users AS 
 SELECT orders.id,
    orders.fkuserid,
    orders.commoditytype,
    orders.amount,
    orders.price,
    orders.orderdate,
    orders.fkmatchingorderid,
    users.id AS userid,
	users.name As username,
    users.longitude,
    users.latitude,
    commodity.name as commodityname,
	orders.ordertype,
   FROM orders
     JOIN users ON orders.fkuserid = users.id
     JOIN commoditytype commodity on orders.commoditytype = commodity.id;

ALTER TABLE orders_users
  OWNER TO postgres;

CREATE TABLE commoditytype
(
  id integer NOT NULL,
  name character varying,
  CONSTRAINT commoditytype_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE commoditytype
  OWNER TO postgres;

-- Table: outboxes

-- DROP TABLE outboxes;

CREATE TABLE outboxes
(
  id serial NOT NULL,
  phonenumber character varying,
  message character varying,
  isverified boolean,
  CONSTRAINT outboxes_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE outboxes
  OWNER TO postgres;
