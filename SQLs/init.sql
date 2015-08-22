  -- Table: orders

-- DROP TABLE orders;

CREATE TABLE orders
(
  id serial NOT NULL,
  fkuserid integer,
  commoditytype integer,
  amount integer,
  price integer,
  order_type character varying,
  orderdate timestamp without time zone,
  fkmatchingorderid integer,
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
  isverified bit(1),
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
    users.longitude,
    users.latitude
   FROM orders
     JOIN users ON orders.fkuserid = users.id;

ALTER TABLE orders_users
  OWNER TO postgres;

CREATE TABLE commodity_types
(
  id integer,
  name character varying(255)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE commodity_types
  OWNER TO postgres;

