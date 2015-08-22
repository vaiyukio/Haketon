-- Table: test

-- DROP TABLE test;

CREATE TABLE test
(
  id bigint NOT NULL DEFAULT nextval('"test_id_seq"'::regclass),
  name character varying(255),
  CONSTRAINT "Test_pkey" PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE test
  OWNER TO postgres;