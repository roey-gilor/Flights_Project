--
-- PostgreSQL database dump
--

-- Dumped from database version 13.3
-- Dumped by pg_dump version 13.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: sp_add_admin(text, text, integer, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_admin(_first_name text, _last_name text, _level integer, _user_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
Declare
    new_id bigint;
   begin 
	   insert into administrators (first_name,last_name,"level",user_id) 
	   values (_first_name,_last_name,_level,_user_id) returning id into new_id;
	   return new_id;
   end;
   $$;


ALTER FUNCTION public.sp_add_admin(_first_name text, _last_name text, _level integer, _user_id bigint) OWNER TO postgres;

--
-- Name: sp_add_airline(text, bigint, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_airline(_name text, _country_id bigint, _user_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
Declare
    new_id bigint;
     begin 
	     insert into airline_companies (name,country_id,user_id) 
		 values (_name,_country_id,_user_id) returning id into new_id;
		 return new_id;
     end;
$$;


ALTER FUNCTION public.sp_add_airline(_name text, _country_id bigint, _user_id bigint) OWNER TO postgres;

--
-- Name: sp_add_country(text); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_add_country(_name text)
    LANGUAGE plpgsql
    AS $$
   begin 
	   insert into countries (name) values (_name);
   end
$$;


ALTER PROCEDURE public.sp_add_country(_name text) OWNER TO postgres;

--
-- Name: sp_add_customer(text, text, text, text, text, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
Declare
    new_id bigint;
    begin 
	    insert into customers(first_name,last_name,address,phone_no,credit_card_no,user_id)
		values (_first_name,_last_name,_address,_phone_no,_credit_card_no,_user_id) returning id into new_id;
    return new_id;
	end;
$$;


ALTER FUNCTION public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint) OWNER TO postgres;

--
-- Name: sp_add_flight(bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_flight(_airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
Declare
    new_id bigint;
     begin 
	     insert into flights (airline_company_id,origin_country_id,destination_country_id,departure_time,landing_time,remaining_tickets)
	     values (_airline_company_id,_origin_country_id,_destination_country_id,_departure_time,_landing_time,_remaining_tickets)
		 returning id into new_id;
		return new_id;
     end;
$$;


ALTER FUNCTION public.sp_add_flight(_airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer) OWNER TO postgres;

--
-- Name: sp_add_flight_into_flights_history(bigint, bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_add_flight_into_flights_history(_flight_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into flights_history (flight_id ,airline_company_id,origin_country_id,destination_country_id,departure_time,landing_time,remaining_tickets)
	     values (_flight_id,_airline_company_id,_origin_country_id,_destination_country_id,_departure_time,_landing_time,_remaining_tickets);
     end;
$$;


ALTER PROCEDURE public.sp_add_flight_into_flights_history(_flight_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer) OWNER TO postgres;

--
-- Name: sp_add_ticket(bigint, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_ticket(_flight_id bigint, _customer_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
 Declare
    new_id bigint;
     begin 
	     insert into tickets (flight_id,customer_id) values (_flight_id,_customer_id) returning id into new_id;
	 return new_id;
	 end;
$$;


ALTER FUNCTION public.sp_add_ticket(_flight_id bigint, _customer_id bigint) OWNER TO postgres;

--
-- Name: sp_add_ticket_into_tickets_history(bigint, bigint, bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_add_ticket_into_tickets_history(_ticket_id bigint, _flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into tickets_history (ticket_id ,flight_id,customer_id) values (_ticket_id,_flight_id,_customer_id);
     end;
$$;


ALTER PROCEDURE public.sp_add_ticket_into_tickets_history(_ticket_id bigint, _flight_id bigint, _customer_id bigint) OWNER TO postgres;

--
-- Name: sp_add_user(text, text, text, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_user(_user_name text, _password text, _email text, _user_role bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
    Declare
    new_id bigint;
     begin 
	     insert into users (username,"password",email,user_role) values
	    (_user_name,_password,_email,_user_role) returning id into new_id;
	   return new_id;
     end;
$$;


ALTER FUNCTION public.sp_add_user(_user_name text, _password text, _email text, _user_role bigint) OWNER TO postgres;

--
-- Name: sp_add_waiting_admin(text, text, integer, text, text, text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_waiting_admin(_first_name text, _last_name text, _level integer, _username text, _password text, _email text) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
Declare
    new_id bigint;
     begin 
	     insert into waiting_admins (first_name,last_name,level,username,password,email) 
		 values (_first_name,_last_name,_level,_username,_password,_email) returning id into new_id;
		 return new_id;
     end;
$$;


ALTER FUNCTION public.sp_add_waiting_admin(_first_name text, _last_name text, _level integer, _username text, _password text, _email text) OWNER TO postgres;

--
-- Name: sp_add_waiting_airline(text, bigint, text, text, text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_add_waiting_airline(_airline_name text, _country_id bigint, _username text, _password text, _email text) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
Declare
    new_id bigint;
     begin 
	     insert into waiting_airlines (airline_name,country_id,username,password,email) 
		 values (_airline_name,_country_id,_username,_password,_email) returning id into new_id;
		 return new_id;
     end;
$$;


ALTER FUNCTION public.sp_add_waiting_airline(_airline_name text, _country_id bigint, _username text, _password text, _email text) OWNER TO postgres;

--
-- Name: sp_get_admin_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_admin_by_id(_id bigint) RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators where administrators.id=_id;
   end;
$$;


ALTER FUNCTION public.sp_get_admin_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_admin_by_user_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_admin_by_user_id(_userid bigint) RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators where administrators.user_id=_userId;
   end;
$$;


ALTER FUNCTION public.sp_get_admin_by_user_id(_userid bigint) OWNER TO postgres;

--
-- Name: sp_get_airline_by_country(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_airline_by_country(_country_id bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies ac where ac.country_id = (select countries.id from countries where countries.id = _country_id);
     end;
$$;


ALTER FUNCTION public.sp_get_airline_by_country(_country_id bigint) OWNER TO postgres;

--
-- Name: sp_get_airline_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_airline_by_id(_id bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies where airline_companies.id=_id;
     end;
$$;


ALTER FUNCTION public.sp_get_airline_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_airline_by_user_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_airline_by_user_id(_userid bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies where airline_companies.user_id=_userId;
     end;
$$;


ALTER FUNCTION public.sp_get_airline_by_user_id(_userid bigint) OWNER TO postgres;

--
-- Name: sp_get_airline_by_user_name(text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_airline_by_user_name(_user_name text) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies ac where ac.user_id = (select users.id from users where users.username=_user_name);
     end;
$$;


ALTER FUNCTION public.sp_get_airline_by_user_name(_user_name text) OWNER TO postgres;

--
-- Name: sp_get_all_admins(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_admins() RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators;
   end;
$$;


ALTER FUNCTION public.sp_get_all_admins() OWNER TO postgres;

--
-- Name: sp_get_all_airlines(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_airlines() RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies;
     end;
$$;


ALTER FUNCTION public.sp_get_all_airlines() OWNER TO postgres;

--
-- Name: sp_get_all_countries(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_countries() RETURNS TABLE(id bigint, name text)
    LANGUAGE plpgsql
    AS $$
  begin
	  return query
	  select * from countries;
  end
$$;


ALTER FUNCTION public.sp_get_all_countries() OWNER TO postgres;

--
-- Name: sp_get_all_customers(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_customers() RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers;
    end;
$$;


ALTER FUNCTION public.sp_get_all_customers() OWNER TO postgres;

--
-- Name: sp_get_all_flights(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_flights() RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights;
     end;
$$;


ALTER FUNCTION public.sp_get_all_flights() OWNER TO postgres;

--
-- Name: sp_get_all_tickets(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_tickets() RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from tickets;
   end;
$$;


ALTER FUNCTION public.sp_get_all_tickets() OWNER TO postgres;

--
-- Name: sp_get_all_users(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_users() RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users;
    end;
$$;


ALTER FUNCTION public.sp_get_all_users() OWNER TO postgres;

--
-- Name: sp_get_all_vacancy_flights(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_vacancy_flights() RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.remaining_tickets >0;
     end;
$$;


ALTER FUNCTION public.sp_get_all_vacancy_flights() OWNER TO postgres;

--
-- Name: sp_get_all_waiting_admins(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_waiting_admins() RETURNS TABLE(id bigint, first_name text, last_name text, level integer, username text, password text, email text)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from waiting_admins;
     end;
$$;


ALTER FUNCTION public.sp_get_all_waiting_admins() OWNER TO postgres;

--
-- Name: sp_get_all_waiting_airlines(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_waiting_airlines() RETURNS TABLE(id bigint, airline_name text, country_id bigint, username text, password text, email text)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from waiting_airlines;
     end;
$$;


ALTER FUNCTION public.sp_get_all_waiting_airlines() OWNER TO postgres;

--
-- Name: sp_get_country_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_country_by_id(_id bigint) RETURNS TABLE(id bigint, name text)
    LANGUAGE plpgsql
    AS $$
  begin
	  return query
	  select * from countries where countries.id=_id;
  end
$$;


ALTER FUNCTION public.sp_get_country_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_customer_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_customer_by_id(_id bigint) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.id=_id;
    end;
$$;


ALTER FUNCTION public.sp_get_customer_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_customer_by_user_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_customer_by_user_id(_userid bigint) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.user_id=_userId;
    end;
$$;


ALTER FUNCTION public.sp_get_customer_by_user_id(_userid bigint) OWNER TO postgres;

--
-- Name: sp_get_customer_by_user_name(text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_customer_by_user_name(_user_name text) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.user_id=(
select u.id from customers c join users u on c.user_id =u.id where u.username =_user_name);
    end;
$$;


ALTER FUNCTION public.sp_get_customer_by_user_name(_user_name text) OWNER TO postgres;

--
-- Name: sp_get_flight_by_customer(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flight_by_customer(_customer_id bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where f.id in (select t.flight_id from tickets t where t.customer_id = _customer_id);
     end;
$$;


ALTER FUNCTION public.sp_get_flight_by_customer(_customer_id bigint) OWNER TO postgres;

--
-- Name: sp_get_flight_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flight_by_id(_id bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.id=_id;
     end;
$$;


ALTER FUNCTION public.sp_get_flight_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_flights_before_datetime(timestamp without time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flights_before_datetime(_landing_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.landing_time as timestamp) < cast (_landing_time as timestamp);
     end;
    $$;


ALTER FUNCTION public.sp_get_flights_before_datetime(_landing_time timestamp without time zone) OWNER TO postgres;

--
-- Name: sp_get_flights_by_departure_date(timestamp without time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flights_by_departure_date(_departure_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.departure_time as date)=cast (_departure_time as date);
     end;
$$;


ALTER FUNCTION public.sp_get_flights_by_departure_date(_departure_time timestamp without time zone) OWNER TO postgres;

--
-- Name: sp_get_flights_by_destination_country(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flights_by_destination_country(_country_code bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.destination_country_id =_country_code;
     end;
$$;


ALTER FUNCTION public.sp_get_flights_by_destination_country(_country_code bigint) OWNER TO postgres;

--
-- Name: sp_get_flights_by_landing_date(timestamp without time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flights_by_landing_date(_landing_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.landing_time as date) =cast (_landing_time as date);
     end;
$$;


ALTER FUNCTION public.sp_get_flights_by_landing_date(_landing_time timestamp without time zone) OWNER TO postgres;

--
-- Name: sp_get_flights_by_origin_country(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_flights_by_origin_country(_country_code bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.origin_country_id =_country_code;
     end;
$$;


ALTER FUNCTION public.sp_get_flights_by_origin_country(_country_code bigint) OWNER TO postgres;

--
-- Name: sp_get_ticket_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_ticket_by_id(_id bigint) RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from tickets where tickets.id=_id;
   end;
$$;


ALTER FUNCTION public.sp_get_ticket_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_tickets_by_flight(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_tickets_by_flight(_flight_id bigint) RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select tickets.id, tickets.flight_id, tickets.customer_id from tickets inner join flights on
tickets.flight_id = flights.id where tickets.flight_id = _flight_id;
   end;
$$;


ALTER FUNCTION public.sp_get_tickets_by_flight(_flight_id bigint) OWNER TO postgres;

--
-- Name: sp_get_user_by_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_user_by_id(_id bigint) RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users where users.id = _id;
    end;
$$;


ALTER FUNCTION public.sp_get_user_by_id(_id bigint) OWNER TO postgres;

--
-- Name: sp_get_user_by_username(text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_user_by_username(_user_name text) RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users where users.username = _user_name;
    end;
$$;


ALTER FUNCTION public.sp_get_user_by_username(_user_name text) OWNER TO postgres;

--
-- Name: sp_remove_admin(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_admin(_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   delete from administrators where id=_id;
   end;
$$;


ALTER PROCEDURE public.sp_remove_admin(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_airline(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_airline(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from airline_companies where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_airline(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_country(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_country(_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   delete from countries where id=_id;
   end
$$;


ALTER PROCEDURE public.sp_remove_country(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_customer(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_customer(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from customers where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_customer(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_flight(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_flight(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from flights where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_flight(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_ticket(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_ticket(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from tickets where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_ticket(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_user(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_user(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from users where id = _id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_user(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_waiting_admin(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_waiting_admin(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from waiting_admins where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_waiting_admin(_id bigint) OWNER TO postgres;

--
-- Name: sp_remove_waiting_airline(bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_remove_waiting_airline(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from waiting_airlines where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_remove_waiting_airline(_id bigint) OWNER TO postgres;

--
-- Name: sp_update_admin(bigint, text, text, integer, bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_admin(_id bigint, _first_name text, _last_name text, _level integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   update administrators set first_name = _first_name, last_name = _last_name, "level" =_level, user_id = _user_id where id=_id;
   end;
$$;


ALTER PROCEDURE public.sp_update_admin(_id bigint, _first_name text, _last_name text, _level integer, _user_id bigint) OWNER TO postgres;

--
-- Name: sp_update_airline(bigint, text, bigint, bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_airline(_id bigint, _name text, _country_id bigint, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update airline_companies set "name" =_name,country_id =_country_id,user_id =_user_id where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_update_airline(_id bigint, _name text, _country_id bigint, _user_id bigint) OWNER TO postgres;

--
-- Name: sp_update_country(bigint, text); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_country(_id bigint, _name text)
    LANGUAGE plpgsql
    AS $$
   begin 
	   update countries set "name" =_name where id=_id;
   end
$$;


ALTER PROCEDURE public.sp_update_country(_id bigint, _name text) OWNER TO postgres;

--
-- Name: sp_update_customer(bigint, text, text, text, text, text, bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    update customers set first_name =_first_name,last_name =_last_name,address =_address,phone_no =_phone_no,credit_card_no =_credit_card_no,user_id =_user_id where id=_id;
    end;
$$;


ALTER PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint) OWNER TO postgres;

--
-- Name: sp_update_flight(bigint, bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
      begin 
	      update flights set airline_company_id =_airline_company_id,origin_country_id =_origin_country_id,destination_country_id =_destination_country_id,
	      departure_time = _departure_time,landing_time =_landing_time,remaining_tickets =_remaining_tickets where id=_id;
      end;
$$;


ALTER PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer) OWNER TO postgres;

--
-- Name: sp_update_ticket(bigint, bigint, bigint); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update tickets set flight_id = _flight_id, customer_id = _customer_id where id=_id;
     end;
$$;


ALTER PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint) OWNER TO postgres;

--
-- Name: sp_update_user(bigint, text, text, text, integer); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.sp_update_user(_id bigint, _user_name text, _password text, _email text, _user_role integer)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update users set username = _user_name, "password" =_password, email = _email, user_role = _user_role where id = _id;
     end;
$$;


ALTER PROCEDURE public.sp_update_user(_id bigint, _user_name text, _password text, _email text, _user_role integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: administrators; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.administrators (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    level integer NOT NULL,
    user_id bigint NOT NULL
);


ALTER TABLE public.administrators OWNER TO postgres;

--
-- Name: administrators_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.administrators_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.administrators_id_seq OWNER TO postgres;

--
-- Name: administrators_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.administrators_id_seq OWNED BY public.administrators.id;


--
-- Name: airline_companies; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.airline_companies (
    id bigint NOT NULL,
    name text NOT NULL,
    country_id bigint NOT NULL,
    user_id bigint NOT NULL
);


ALTER TABLE public.airline_companies OWNER TO postgres;

--
-- Name: airline_companies_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.airline_companies_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.airline_companies_id_seq OWNER TO postgres;

--
-- Name: airline_companies_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.airline_companies_id_seq OWNED BY public.airline_companies.id;


--
-- Name: countries; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.countries (
    id bigint NOT NULL,
    name text NOT NULL
);


ALTER TABLE public.countries OWNER TO postgres;

--
-- Name: countries_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.countries_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.countries_id_seq OWNER TO postgres;

--
-- Name: countries_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.countries_id_seq OWNED BY public.countries.id;


--
-- Name: customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customers (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    address text NOT NULL,
    phone_no text NOT NULL,
    credit_card_no text NOT NULL,
    user_id bigint NOT NULL
);


ALTER TABLE public.customers OWNER TO postgres;

--
-- Name: customers_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.customers_id_seq OWNER TO postgres;

--
-- Name: customers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customers_id_seq OWNED BY public.customers.id;


--
-- Name: flights; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.flights (
    id bigint NOT NULL,
    airline_company_id bigint NOT NULL,
    origin_country_id bigint NOT NULL,
    destination_country_id bigint NOT NULL,
    departure_time timestamp(0) without time zone NOT NULL,
    landing_time timestamp(0) without time zone NOT NULL,
    remaining_tickets integer NOT NULL
);


ALTER TABLE public.flights OWNER TO postgres;

--
-- Name: flights_history; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.flights_history (
    id bigint NOT NULL,
    flight_id bigint NOT NULL,
    airline_company_id bigint NOT NULL,
    origin_country_id bigint NOT NULL,
    destination_country_id bigint NOT NULL,
    departure_time timestamp without time zone NOT NULL,
    landing_time timestamp without time zone NOT NULL,
    remaining_tickets integer NOT NULL
);


ALTER TABLE public.flights_history OWNER TO postgres;

--
-- Name: flights_history_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.flights_history_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.flights_history_id_seq OWNER TO postgres;

--
-- Name: flights_history_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.flights_history_id_seq OWNED BY public.flights_history.id;


--
-- Name: flights_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.flights_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.flights_id_seq OWNER TO postgres;

--
-- Name: flights_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.flights_id_seq OWNED BY public.flights.id;


--
-- Name: tickets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tickets (
    id bigint NOT NULL,
    flight_id bigint NOT NULL,
    customer_id bigint NOT NULL
);


ALTER TABLE public.tickets OWNER TO postgres;

--
-- Name: tickets_history; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tickets_history (
    id bigint NOT NULL,
    ticket_id bigint NOT NULL,
    flight_id bigint NOT NULL,
    customer_id bigint NOT NULL
);


ALTER TABLE public.tickets_history OWNER TO postgres;

--
-- Name: tickets_history_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tickets_history_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tickets_history_id_seq OWNER TO postgres;

--
-- Name: tickets_history_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tickets_history_id_seq OWNED BY public.tickets_history.id;


--
-- Name: tickets_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tickets_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tickets_id_seq OWNER TO postgres;

--
-- Name: tickets_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tickets_id_seq OWNED BY public.tickets.id;


--
-- Name: user_roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_roles (
    id integer NOT NULL,
    role_name text NOT NULL
);


ALTER TABLE public.user_roles OWNER TO postgres;

--
-- Name: user_roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_roles_id_seq OWNER TO postgres;

--
-- Name: user_roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_roles_id_seq OWNED BY public.user_roles.id;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id bigint NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    email text NOT NULL,
    user_role integer NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_id_seq OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- Name: users_user_role_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_user_role_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_user_role_seq OWNER TO postgres;

--
-- Name: users_user_role_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_role_seq OWNED BY public.users.user_role;


--
-- Name: waiting_admins; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.waiting_admins (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    level integer NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    email text NOT NULL
);


ALTER TABLE public.waiting_admins OWNER TO postgres;

--
-- Name: waiting_admins_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.waiting_admins_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.waiting_admins_id_seq OWNER TO postgres;

--
-- Name: waiting_admins_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.waiting_admins_id_seq OWNED BY public.waiting_admins.id;


--
-- Name: waiting_airlines; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.waiting_airlines (
    id bigint NOT NULL,
    airline_name text NOT NULL,
    country_id bigint NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    email text NOT NULL
);


ALTER TABLE public.waiting_airlines OWNER TO postgres;

--
-- Name: waiting_airlines_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.waiting_airlines_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.waiting_airlines_id_seq OWNER TO postgres;

--
-- Name: waiting_airlines_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.waiting_airlines_id_seq OWNED BY public.waiting_airlines.id;


--
-- Name: administrators id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrators ALTER COLUMN id SET DEFAULT nextval('public.administrators_id_seq'::regclass);


--
-- Name: airline_companies id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.airline_companies ALTER COLUMN id SET DEFAULT nextval('public.airline_companies_id_seq'::regclass);


--
-- Name: countries id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.countries ALTER COLUMN id SET DEFAULT nextval('public.countries_id_seq'::regclass);


--
-- Name: customers id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers ALTER COLUMN id SET DEFAULT nextval('public.customers_id_seq'::regclass);


--
-- Name: flights id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights ALTER COLUMN id SET DEFAULT nextval('public.flights_id_seq'::regclass);


--
-- Name: flights_history id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights_history ALTER COLUMN id SET DEFAULT nextval('public.flights_history_id_seq'::regclass);


--
-- Name: tickets id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tickets ALTER COLUMN id SET DEFAULT nextval('public.tickets_id_seq'::regclass);


--
-- Name: tickets_history id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tickets_history ALTER COLUMN id SET DEFAULT nextval('public.tickets_history_id_seq'::regclass);


--
-- Name: user_roles id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles ALTER COLUMN id SET DEFAULT nextval('public.user_roles_id_seq'::regclass);


--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- Name: users user_role; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_role SET DEFAULT nextval('public.users_user_role_seq'::regclass);


--
-- Name: waiting_admins id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.waiting_admins ALTER COLUMN id SET DEFAULT nextval('public.waiting_admins_id_seq'::regclass);


--
-- Name: waiting_airlines id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.waiting_airlines ALTER COLUMN id SET DEFAULT nextval('public.waiting_airlines_id_seq'::regclass);


--
-- Data for Name: administrators; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.administrators (id, first_name, last_name, level, user_id) FROM stdin;
1	roey	levy	1	1
2	amir	cohen	2	2
3	danny	sela	3	9
\.


--
-- Data for Name: airline_companies; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.airline_companies (id, name, country_id, user_id) FROM stdin;
1	Delta	2	4
4	El-Al	1	3
\.


--
-- Data for Name: countries; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.countries (id, name) FROM stdin;
1	Israel
2	United states
3	Franch
4	Italy
\.


--
-- Data for Name: customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.customers (id, first_name, last_name, address, phone_no, credit_card_no, user_id) FROM stdin;
1	Uri	Shalom	haneher 5	050-9578134	1287	5
2	shany	bar	hanashi 3	050-1547896	4583	6
\.


--
-- Data for Name: flights; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.flights (id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) FROM stdin;
1	1	1	2	2021-01-01 05:00:00	2021-01-01 16:00:00	4
2	1	1	3	2021-01-23 15:00:00	2021-01-23 19:00:00	12
6	4	2	4	2021-01-13 09:00:00	2021-01-13 20:00:00	5
7	4	2	3	2021-01-24 10:00:00	2021-01-24 14:00:00	0
\.


--
-- Data for Name: flights_history; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.flights_history (id, flight_id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) FROM stdin;
\.


--
-- Data for Name: tickets; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tickets (id, flight_id, customer_id) FROM stdin;
1	1	1
2	1	2
3	2	2
4	2	1
\.


--
-- Data for Name: tickets_history; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tickets_history (id, ticket_id, flight_id, customer_id) FROM stdin;
\.


--
-- Data for Name: user_roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_roles (id, role_name) FROM stdin;
1	Administrator
3	Customer
2	Airline Company
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, password, email, user_role) FROM stdin;
1	roey123	12345	roy@gmail.com	1
2	amir54	fsdf3	amir@gmail.com	1
3	adi213	54321	adi@gmail.com	2
4	dana432	gdfds	sana@gmail.com	2
5	uri321	vzd474	uri@gmail.com	3
6	shany805	dsaasa	shany@gmail.com	3
9	danny121121	fdsaa23	danny@gmail.com	1
\.


--
-- Data for Name: waiting_admins; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.waiting_admins (id, first_name, last_name, level, username, password, email) FROM stdin;
1	Anna	Levin	2	anna576	rtgvvr3	anna@walla.com
\.


--
-- Data for Name: waiting_airlines; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.waiting_airlines (id, airline_name, country_id, username, password, email) FROM stdin;
1	Ista	1	Boaz23	kfutr29j	boaz@walla.com
\.


--
-- Name: administrators_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.administrators_id_seq', 4, true);


--
-- Name: airline_companies_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.airline_companies_id_seq', 5, true);


--
-- Name: countries_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.countries_id_seq', 6, true);


--
-- Name: customers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.customers_id_seq', 4, true);


--
-- Name: flights_history_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.flights_history_id_seq', 1, true);


--
-- Name: flights_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.flights_id_seq', 8, true);


--
-- Name: tickets_history_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tickets_history_id_seq', 1, true);


--
-- Name: tickets_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tickets_id_seq', 5, true);


--
-- Name: user_roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_roles_id_seq', 4, true);


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 10, true);


--
-- Name: users_user_role_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_role_seq', 1, false);


--
-- Name: waiting_admins_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.waiting_admins_id_seq', 6, true);


--
-- Name: waiting_airlines_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.waiting_airlines_id_seq', 5, true);


--
-- Name: administrators administrators_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_pk PRIMARY KEY (id);


--
-- Name: airline_companies airline_companies_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_pk PRIMARY KEY (id);


--
-- Name: countries countries_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_pk PRIMARY KEY (id);


--
-- Name: customers customers_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pk PRIMARY KEY (id);


--
-- Name: flights_history flights_history_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights_history
    ADD CONSTRAINT flights_history_pk PRIMARY KEY (id);


--
-- Name: flights flights_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_pk PRIMARY KEY (id);


--
-- Name: tickets_history tickets_history_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tickets_history
    ADD CONSTRAINT tickets_history_pk PRIMARY KEY (id);


--
-- Name: tickets tickets_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_pk PRIMARY KEY (id);


--
-- Name: user_roles user_roles_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_pk PRIMARY KEY (id);


--
-- Name: users users_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);


--
-- Name: waiting_admins waiting_admins_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.waiting_admins
    ADD CONSTRAINT waiting_admins_pk PRIMARY KEY (id);


--
-- Name: waiting_airlines waiting_airlines_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.waiting_airlines
    ADD CONSTRAINT waiting_airlines_pk PRIMARY KEY (id);


--
-- Name: administrators_user_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX administrators_user_id_uindex ON public.administrators USING btree (user_id);


--
-- Name: airline_companies_name_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX airline_companies_name_uindex ON public.airline_companies USING btree (name);


--
-- Name: airline_companies_user_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX airline_companies_user_id_uindex ON public.airline_companies USING btree (user_id);


--
-- Name: countries_name_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX countries_name_uindex ON public.countries USING btree (name);


--
-- Name: customers_credit_card_no_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX customers_credit_card_no_uindex ON public.customers USING btree (credit_card_no);


--
-- Name: customers_phone_no_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX customers_phone_no_uindex ON public.customers USING btree (phone_no);


--
-- Name: customers_user_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX customers_user_id_uindex ON public.customers USING btree (user_id);


--
-- Name: flights_history_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX flights_history_id_uindex ON public.flights_history USING btree (id);


--
-- Name: tickets_flight_id_customer_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX tickets_flight_id_customer_id_uindex ON public.tickets USING btree (flight_id, customer_id);


--
-- Name: tickets_history_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX tickets_history_id_uindex ON public.tickets_history USING btree (id);


--
-- Name: user_roles_role_name_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX user_roles_role_name_uindex ON public.user_roles USING btree (role_name);


--
-- Name: users_email_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX users_email_uindex ON public.users USING btree (email);


--
-- Name: users_username_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX users_username_uindex ON public.users USING btree (username);


--
-- Name: waiting_admins_email_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_admins_email_uindex ON public.waiting_admins USING btree (email);


--
-- Name: waiting_admins_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_admins_id_uindex ON public.waiting_admins USING btree (id);


--
-- Name: waiting_admins_username_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_admins_username_uindex ON public.waiting_admins USING btree (username);


--
-- Name: waiting_airlines_airline_name_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_airlines_airline_name_uindex ON public.waiting_airlines USING btree (airline_name);


--
-- Name: waiting_airlines_email_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_airlines_email_uindex ON public.waiting_airlines USING btree (email);


--
-- Name: waiting_airlines_id_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_airlines_id_uindex ON public.waiting_airlines USING btree (id);


--
-- Name: waiting_airlines_username_uindex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX waiting_airlines_username_uindex ON public.waiting_airlines USING btree (username);


--
-- Name: administrators administrators_users_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: airline_companies airline_companies_countries_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_countries_id_fk FOREIGN KEY (country_id) REFERENCES public.countries(id);


--
-- Name: airline_companies airline_companies_users_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: customers customers_users_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- Name: flights flights_airline_companies_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_airline_companies_id_fk FOREIGN KEY (airline_company_id) REFERENCES public.airline_companies(id);


--
-- Name: flights flights_countries_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk FOREIGN KEY (origin_country_id) REFERENCES public.countries(id);


--
-- Name: flights flights_countries_id_fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk_2 FOREIGN KEY (destination_country_id) REFERENCES public.countries(id);


--
-- Name: tickets tickets_customers_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_customers_id_fk FOREIGN KEY (customer_id) REFERENCES public.customers(id);


--
-- Name: tickets tickets_flights_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_flights_id_fk FOREIGN KEY (flight_id) REFERENCES public.flights(id);


--
-- Name: users users_user_roles_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_user_roles_id_fk FOREIGN KEY (user_role) REFERENCES public.user_roles(id);


--
-- Name: waiting_airlines waiting_airlines_countries_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.waiting_airlines
    ADD CONSTRAINT waiting_airlines_countries_id_fk FOREIGN KEY (country_id) REFERENCES public.countries(id);


--
-- PostgreSQL database dump complete
--

