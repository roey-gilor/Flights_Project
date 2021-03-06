PGDMP     ;    "                y            flights management    13.1    13.1 �    C           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            D           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            E           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            F           1262    16785    flights management    DATABASE     p   CREATE DATABASE "flights management" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Hebrew_Israel.1255';
 $   DROP DATABASE "flights management";
                postgres    false                        2615    2200    public    SCHEMA        CREATE SCHEMA public;
    DROP SCHEMA public;
                postgres    false            G           0    0    SCHEMA public    COMMENT     6   COMMENT ON SCHEMA public IS 'standard public schema';
                   postgres    false    3            �            1255    16954 )   sp_add_admin(text, text, integer, bigint) 	   PROCEDURE       CREATE PROCEDURE public.sp_add_admin(_first_name text, _last_name text, _level integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   insert into administrators (first_name,last_name,"level",user_id) values (_first_name,_last_name,_level,_user_id);
   end;
   $$;
 h   DROP PROCEDURE public.sp_add_admin(_first_name text, _last_name text, _level integer, _user_id bigint);
       public          postgres    false    3            �            1255    17041 $   sp_add_airline(text, bigint, bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_add_airline(_name text, _country_id bigint, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into airline_companies (name,country_id,user_id) values (_name,_country_id,_user_id);
     end;
$$;
 W   DROP PROCEDURE public.sp_add_airline(_name text, _country_id bigint, _user_id bigint);
       public          postgres    false    3            �            1255    16948    sp_add_country(text) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_add_country(_name text)
    LANGUAGE plpgsql
    AS $$
   begin 
	   insert into countries (name) values (_name);
   end
$$;
 2   DROP PROCEDURE public.sp_add_country(_name text);
       public          postgres    false    3            �            1255    17035 5   sp_add_customer(text, text, text, text, text, bigint) 	   PROCEDURE     r  CREATE PROCEDURE public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    insert into customers(first_name,last_name,address,phone_no,credit_card_no,user_id) values (_first_name,_last_name,_address,_phone_no,_credit_card_no,_user_id);
    end;
$$;
 �   DROP PROCEDURE public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint);
       public          postgres    false    3                       1255    17052 h   sp_add_flight(bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer) 	   PROCEDURE     5  CREATE PROCEDURE public.sp_add_flight(_airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into flights (airline_company_id,origin_country_id,destination_country_id,departure_time,landing_time,remaining_tickets)
	     values (_airline_company_id,_origin_country_id,_destination_country_id,_departure_time,_landing_time,_remaining_tickets);
     end;
$$;
 �   DROP PROCEDURE public.sp_add_flight(_airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false    3            �            1255    17017    sp_add_ticket(bigint, bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_add_ticket(_flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into tickets (flight_id,customer_id) values (_flight_id,_customer_id);
     end;
$$;
 M   DROP PROCEDURE public.sp_add_ticket(_flight_id bigint, _customer_id bigint);
       public          postgres    false    3            �            1255    17002 %   sp_add_user(text, text, text, bigint) 	   PROCEDURE       CREATE PROCEDURE public.sp_add_user(_user_name text, _password text, _email text, _user_role bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into users (username,"password",email,user_role) values (_user_name,_password,_email,_user_role);
     end;
$$;
 d   DROP PROCEDURE public.sp_add_user(_user_name text, _password text, _email text, _user_role bigint);
       public          postgres    false    3            �            1255    16956    sp_get_admin_by_id(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_admin_by_id(_id bigint) RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators where administrators.id=_id;
   end;
$$;
 5   DROP FUNCTION public.sp_get_admin_by_id(_id bigint);
       public          postgres    false    3                       1255    17045 !   sp_get_airline_by_country(bigint)    FUNCTION     b  CREATE FUNCTION public.sp_get_airline_by_country(_country_id bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies ac where ac.country_id = (select countries.id from countries where countries.id = _country_id);
     end;
$$;
 D   DROP FUNCTION public.sp_get_airline_by_country(_country_id bigint);
       public          postgres    false    3            �            1255    17040    sp_get_airline_by_id(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_airline_by_id(_id bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies where airline_companies.id=_id;
     end;
$$;
 7   DROP FUNCTION public.sp_get_airline_by_id(_id bigint);
       public          postgres    false    3                       1255    17044 !   sp_get_airline_by_user_name(text)    FUNCTION     U  CREATE FUNCTION public.sp_get_airline_by_user_name(_user_name text) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies ac where ac.user_id = (select users.id from users where users.username=_user_name);
     end;
$$;
 C   DROP FUNCTION public.sp_get_airline_by_user_name(_user_name text);
       public          postgres    false    3            �            1255    16983    sp_get_all_admins()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_admins() RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators;
   end;
$$;
 *   DROP FUNCTION public.sp_get_all_admins();
       public          postgres    false    3                       1255    17046    sp_get_all_airlines()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_airlines() RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies;
     end;
$$;
 ,   DROP FUNCTION public.sp_get_all_airlines();
       public          postgres    false    3            �            1255    16946    sp_get_all_countries()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_countries() RETURNS TABLE(id bigint, name text)
    LANGUAGE plpgsql
    AS $$
  begin
	  return query
	  select * from countries;
  end
$$;
 -   DROP FUNCTION public.sp_get_all_countries();
       public          postgres    false    3            �            1255    17032    sp_get_all_customers()    FUNCTION       CREATE FUNCTION public.sp_get_all_customers() RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers;
    end;
$$;
 -   DROP FUNCTION public.sp_get_all_customers();
       public          postgres    false    3                       1255    17049    sp_get_all_flights()    FUNCTION     w  CREATE FUNCTION public.sp_get_all_flights() RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights;
     end;
$$;
 +   DROP FUNCTION public.sp_get_all_flights();
       public          postgres    false    3            �            1255    17014    sp_get_all_tickets()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_tickets() RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from tickets;
   end;
$$;
 +   DROP FUNCTION public.sp_get_all_tickets();
       public          postgres    false    3            �            1255    16998    sp_get_all_users()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_users() RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users;
    end;
$$;
 )   DROP FUNCTION public.sp_get_all_users();
       public          postgres    false    3                       1255    17064    sp_get_all_vacancy_flights()    FUNCTION     �  CREATE FUNCTION public.sp_get_all_vacancy_flights() RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.remaining_tickets >0;
     end;
$$;
 3   DROP FUNCTION public.sp_get_all_vacancy_flights();
       public          postgres    false    3            �            1255    16947    sp_get_country_by_id(bigint)    FUNCTION     �   CREATE FUNCTION public.sp_get_country_by_id(_id bigint) RETURNS TABLE(id bigint, name text)
    LANGUAGE plpgsql
    AS $$
  begin
	  return query
	  select * from countries where countries.id=_id;
  end
$$;
 7   DROP FUNCTION public.sp_get_country_by_id(_id bigint);
       public          postgres    false    3            �            1255    17033    sp_get_customer_by_id(bigint)    FUNCTION     8  CREATE FUNCTION public.sp_get_customer_by_id(_id bigint) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.id=_id;
    end;
$$;
 8   DROP FUNCTION public.sp_get_customer_by_id(_id bigint);
       public          postgres    false    3            �            1255    17038 "   sp_get_customer_by_user_name(text)    FUNCTION     �  CREATE FUNCTION public.sp_get_customer_by_user_name(_user_name text) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.user_id=(
select u.id from customers c join users u on c.user_id =u.id where u.username =_user_name);
    end;
$$;
 D   DROP FUNCTION public.sp_get_customer_by_user_name(_user_name text);
       public          postgres    false    3                       1255    17063 !   sp_get_flight_by_customer(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flight_by_customer(_customer_id bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where f.id in (select t.flight_id from tickets t where t.customer_id = _customer_id);
     end;
$$;
 E   DROP FUNCTION public.sp_get_flight_by_customer(_customer_id bigint);
       public          postgres    false    3                       1255    17051    sp_get_flight_by_id(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flight_by_id(_id bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.id=_id;
     end;
$$;
 6   DROP FUNCTION public.sp_get_flight_by_id(_id bigint);
       public          postgres    false    3                       1255    17066 =   sp_get_flights_by_departure_date(timestamp without time zone)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_departure_date(_departure_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.departure_time as date)=cast (_departure_time as date);
     end;
$$;
 d   DROP FUNCTION public.sp_get_flights_by_departure_date(_departure_time timestamp without time zone);
       public          postgres    false    3                       1255    17056 -   sp_get_flights_by_destination_country(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_destination_country(_country_code bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.destination_country_id =_country_code;
     end;
$$;
 R   DROP FUNCTION public.sp_get_flights_by_destination_country(_country_code bigint);
       public          postgres    false    3                       1255    17068 ;   sp_get_flights_by_landing_date(timestamp without time zone)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_landing_date(_landing_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.landing_time as date) =cast (_landing_time as date);
     end;
$$;
 `   DROP FUNCTION public.sp_get_flights_by_landing_date(_landing_time timestamp without time zone);
       public          postgres    false    3            
           1255    17055 (   sp_get_flights_by_origin_country(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_origin_country(_country_code bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.origin_country_id =_country_code;
     end;
$$;
 M   DROP FUNCTION public.sp_get_flights_by_origin_country(_country_code bigint);
       public          postgres    false    3            �            1255    17016    sp_get_ticket_by_id(bigint)    FUNCTION     �   CREATE FUNCTION public.sp_get_ticket_by_id(_id bigint) RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from tickets where tickets.id=_id;
   end;
$$;
 6   DROP FUNCTION public.sp_get_ticket_by_id(_id bigint);
       public          postgres    false    3            �            1255    16999    sp_get_user_by_id(bigint)    FUNCTION     	  CREATE FUNCTION public.sp_get_user_by_id(_id bigint) RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users where users.id = _id;
    end;
$$;
 4   DROP FUNCTION public.sp_get_user_by_id(_id bigint);
       public          postgres    false    3            �            1255    16997    sp_remove_admin(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_admin(_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   delete from administrators where id=_id;
   end;
$$;
 3   DROP PROCEDURE public.sp_remove_admin(_id bigint);
       public          postgres    false    3                        1255    17043    sp_remove_airline(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_airline(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from airline_companies where id=_id;
     end;
$$;
 5   DROP PROCEDURE public.sp_remove_airline(_id bigint);
       public          postgres    false    3            �            1255    16951    sp_remove_country(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_country(_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   delete from countries where id=_id;
   end
$$;
 5   DROP PROCEDURE public.sp_remove_country(_id bigint);
       public          postgres    false    3            �            1255    17037    sp_remove_customer(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_customer(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from customers where id=_id;
     end;
$$;
 6   DROP PROCEDURE public.sp_remove_customer(_id bigint);
       public          postgres    false    3            	           1255    17054    sp_remove_flight(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_flight(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from flights where id=_id;
     end;
$$;
 4   DROP PROCEDURE public.sp_remove_flight(_id bigint);
       public          postgres    false    3            �            1255    17019    sp_remove_ticket(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_ticket(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from tickets where id=_id;
     end;
$$;
 4   DROP PROCEDURE public.sp_remove_ticket(_id bigint);
       public          postgres    false    3            �            1255    17004    sp_remove_user(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_user(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from users where id = _id;
     end;
$$;
 2   DROP PROCEDURE public.sp_remove_user(_id bigint);
       public          postgres    false    3            �            1255    16990 4   sp_update_admin(bigint, text, text, integer, bigint) 	   PROCEDURE     3  CREATE PROCEDURE public.sp_update_admin(_id bigint, _first_name text, _last_name text, _level integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   update administrators set first_name = _first_name, last_name = _last_name, "level" =_level, user_id = _user_id where id=_id;
   end;
$$;
 w   DROP PROCEDURE public.sp_update_admin(_id bigint, _first_name text, _last_name text, _level integer, _user_id bigint);
       public          postgres    false    3                       1255    17047 /   sp_update_airline(bigint, text, bigint, bigint) 	   PROCEDURE       CREATE PROCEDURE public.sp_update_airline(_id bigint, _name text, _country_id bigint, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update airline_companies set "name" =_name,country_id =_country_id,user_id =_user_id where id=_id;
     end;
$$;
 f   DROP PROCEDURE public.sp_update_airline(_id bigint, _name text, _country_id bigint, _user_id bigint);
       public          postgres    false    3            �            1255    16949    sp_update_country(bigint, text) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_update_country(_id bigint, _name text)
    LANGUAGE plpgsql
    AS $$
   begin 
	   update countries set "name" =_name where id=_id;
   end
$$;
 A   DROP PROCEDURE public.sp_update_country(_id bigint, _name text);
       public          postgres    false    3            �            1255    17036 @   sp_update_customer(bigint, text, text, text, text, text, bigint) 	   PROCEDURE     �  CREATE PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    update customers set first_name =_first_name,last_name =_last_name,address =_address,phone_no =_phone_no,credit_card_no =_credit_card_no,user_id =_user_id where id=_id;
    end;
$$;
 �   DROP PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint);
       public          postgres    false    3                       1255    17053 s   sp_update_flight(bigint, bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer) 	   PROCEDURE     Q  CREATE PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
      begin 
	      update flights set airline_company_id =_airline_company_id,origin_country_id =_origin_country_id,destination_country_id =_destination_country_id,
	      departure_time = _departure_time,landing_time =_landing_time,remaining_tickets =_remaining_tickets where id=_id;
      end;
$$;
 �   DROP PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false    3            �            1255    17018 (   sp_update_ticket(bigint, bigint, bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update tickets set flight_id = _flight_id, customer_id = _customer_id where id=_id;
     end;
$$;
 \   DROP PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint);
       public          postgres    false    3            �            1255    17006 1   sp_update_user(bigint, text, text, text, integer) 	   PROCEDURE     .  CREATE PROCEDURE public.sp_update_user(_id bigint, _user_name text, _password text, _email text, _user_role integer)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update users set username = _user_name, "password" =_password, email = _email, user_role = _user_role where id = _id;
     end;
$$;
 t   DROP PROCEDURE public.sp_update_user(_id bigint, _user_name text, _password text, _email text, _user_role integer);
       public          postgres    false    3            �            1259    16872    administrators    TABLE     �   CREATE TABLE public.administrators (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    level integer NOT NULL,
    user_id bigint NOT NULL
);
 "   DROP TABLE public.administrators;
       public         heap    postgres    false    3            �            1259    16870    administrators_id_seq    SEQUENCE     �   CREATE SEQUENCE public.administrators_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.administrators_id_seq;
       public          postgres    false    216    3            H           0    0    administrators_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.administrators_id_seq OWNED BY public.administrators.id;
          public          postgres    false    215            �            1259    16817    airline_companies    TABLE     �   CREATE TABLE public.airline_companies (
    id bigint NOT NULL,
    name text NOT NULL,
    country_id bigint NOT NULL,
    user_id bigint NOT NULL
);
 %   DROP TABLE public.airline_companies;
       public         heap    postgres    false    3            �            1259    16815    airline_companies_id_seq    SEQUENCE     �   CREATE SEQUENCE public.airline_companies_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.airline_companies_id_seq;
       public          postgres    false    207    3            I           0    0    airline_companies_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.airline_companies_id_seq OWNED BY public.airline_companies.id;
          public          postgres    false    206            �            1259    16788 	   countries    TABLE     R   CREATE TABLE public.countries (
    id bigint NOT NULL,
    name text NOT NULL
);
    DROP TABLE public.countries;
       public         heap    postgres    false    3            �            1259    16786    countries_id_seq    SEQUENCE     y   CREATE SEQUENCE public.countries_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.countries_id_seq;
       public          postgres    false    201    3            J           0    0    countries_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.countries_id_seq OWNED BY public.countries.id;
          public          postgres    false    200            �            1259    16830 	   customers    TABLE     �   CREATE TABLE public.customers (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    address text NOT NULL,
    phone_no text NOT NULL,
    credit_card_no text NOT NULL,
    user_id bigint NOT NULL
);
    DROP TABLE public.customers;
       public         heap    postgres    false    3            �            1259    16828    customers_id_seq    SEQUENCE     y   CREATE SEQUENCE public.customers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.customers_id_seq;
       public          postgres    false    209    3            K           0    0    customers_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.customers_id_seq OWNED BY public.customers.id;
          public          postgres    false    208            �            1259    16800    flights    TABLE     Q  CREATE TABLE public.flights (
    id bigint NOT NULL,
    airline_company_id bigint NOT NULL,
    origin_country_id bigint NOT NULL,
    destination_country_id bigint NOT NULL,
    departure_time timestamp(0) without time zone NOT NULL,
    landing_time timestamp(0) without time zone NOT NULL,
    remaining_tickets integer NOT NULL
);
    DROP TABLE public.flights;
       public         heap    postgres    false    3            �            1259    16798    flights_id_seq    SEQUENCE     w   CREATE SEQUENCE public.flights_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.flights_id_seq;
       public          postgres    false    203    3            L           0    0    flights_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.flights_id_seq OWNED BY public.flights.id;
          public          postgres    false    202            �            1259    16808    tickets    TABLE     x   CREATE TABLE public.tickets (
    id bigint NOT NULL,
    flight_id bigint NOT NULL,
    customer_id bigint NOT NULL
);
    DROP TABLE public.tickets;
       public         heap    postgres    false    3            �            1259    16806    tickets_id_seq    SEQUENCE     w   CREATE SEQUENCE public.tickets_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.tickets_id_seq;
       public          postgres    false    205    3            M           0    0    tickets_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.tickets_id_seq OWNED BY public.tickets.id;
          public          postgres    false    204            �            1259    16860 
   user_roles    TABLE     Y   CREATE TABLE public.user_roles (
    id integer NOT NULL,
    role_name text NOT NULL
);
    DROP TABLE public.user_roles;
       public         heap    postgres    false    3            �            1259    16858    user_roles_id_seq    SEQUENCE     �   CREATE SEQUENCE public.user_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.user_roles_id_seq;
       public          postgres    false    3    214            N           0    0    user_roles_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.user_roles_id_seq OWNED BY public.user_roles.id;
          public          postgres    false    213            �            1259    16846    users    TABLE     �   CREATE TABLE public.users (
    id bigint NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    email text NOT NULL,
    user_role integer NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false    3            �            1259    16842    users_id_seq    SEQUENCE     u   CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    212    3            O           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    210            �            1259    16844    users_user_role_seq    SEQUENCE     �   CREATE SEQUENCE public.users_user_role_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.users_user_role_seq;
       public          postgres    false    3    212            P           0    0    users_user_role_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.users_user_role_seq OWNED BY public.users.user_role;
          public          postgres    false    211            �           2604    16957    administrators id    DEFAULT     v   ALTER TABLE ONLY public.administrators ALTER COLUMN id SET DEFAULT nextval('public.administrators_id_seq'::regclass);
 @   ALTER TABLE public.administrators ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    216    216            �           2604    16820    airline_companies id    DEFAULT     |   ALTER TABLE ONLY public.airline_companies ALTER COLUMN id SET DEFAULT nextval('public.airline_companies_id_seq'::regclass);
 C   ALTER TABLE public.airline_companies ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    206    207    207            �           2604    16791    countries id    DEFAULT     l   ALTER TABLE ONLY public.countries ALTER COLUMN id SET DEFAULT nextval('public.countries_id_seq'::regclass);
 ;   ALTER TABLE public.countries ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    201    200    201            �           2604    16833    customers id    DEFAULT     l   ALTER TABLE ONLY public.customers ALTER COLUMN id SET DEFAULT nextval('public.customers_id_seq'::regclass);
 ;   ALTER TABLE public.customers ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    208    209    209            �           2604    16803 
   flights id    DEFAULT     h   ALTER TABLE ONLY public.flights ALTER COLUMN id SET DEFAULT nextval('public.flights_id_seq'::regclass);
 9   ALTER TABLE public.flights ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    202    203    203            �           2604    16811 
   tickets id    DEFAULT     h   ALTER TABLE ONLY public.tickets ALTER COLUMN id SET DEFAULT nextval('public.tickets_id_seq'::regclass);
 9   ALTER TABLE public.tickets ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    205    204    205            �           2604    16863    user_roles id    DEFAULT     n   ALTER TABLE ONLY public.user_roles ALTER COLUMN id SET DEFAULT nextval('public.user_roles_id_seq'::regclass);
 <   ALTER TABLE public.user_roles ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    214    213    214            �           2604    16849    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    210    212    212            �           2604    16850    users user_role    DEFAULT     r   ALTER TABLE ONLY public.users ALTER COLUMN user_role SET DEFAULT nextval('public.users_user_role_seq'::regclass);
 >   ALTER TABLE public.users ALTER COLUMN user_role DROP DEFAULT;
       public          postgres    false    212    211    212            @          0    16872    administrators 
   TABLE DATA           S   COPY public.administrators (id, first_name, last_name, level, user_id) FROM stdin;
    public          postgres    false    216            7          0    16817    airline_companies 
   TABLE DATA           J   COPY public.airline_companies (id, name, country_id, user_id) FROM stdin;
    public          postgres    false    207            1          0    16788 	   countries 
   TABLE DATA           -   COPY public.countries (id, name) FROM stdin;
    public          postgres    false    201            9          0    16830 	   customers 
   TABLE DATA           j   COPY public.customers (id, first_name, last_name, address, phone_no, credit_card_no, user_id) FROM stdin;
    public          postgres    false    209            3          0    16800    flights 
   TABLE DATA           �   COPY public.flights (id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) FROM stdin;
    public          postgres    false    203            5          0    16808    tickets 
   TABLE DATA           =   COPY public.tickets (id, flight_id, customer_id) FROM stdin;
    public          postgres    false    205            >          0    16860 
   user_roles 
   TABLE DATA           3   COPY public.user_roles (id, role_name) FROM stdin;
    public          postgres    false    214            <          0    16846    users 
   TABLE DATA           I   COPY public.users (id, username, password, email, user_role) FROM stdin;
    public          postgres    false    212            Q           0    0    administrators_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.administrators_id_seq', 4, true);
          public          postgres    false    215            R           0    0    airline_companies_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.airline_companies_id_seq', 5, true);
          public          postgres    false    206            S           0    0    countries_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.countries_id_seq', 6, true);
          public          postgres    false    200            T           0    0    customers_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.customers_id_seq', 4, true);
          public          postgres    false    208            U           0    0    flights_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.flights_id_seq', 8, true);
          public          postgres    false    202            V           0    0    tickets_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.tickets_id_seq', 5, true);
          public          postgres    false    204            W           0    0    user_roles_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.user_roles_id_seq', 4, true);
          public          postgres    false    213            X           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 10, true);
          public          postgres    false    210            Y           0    0    users_user_role_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.users_user_role_seq', 1, false);
          public          postgres    false    211            �           2606    16959     administrators administrators_pk 
   CONSTRAINT     ^   ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_pk PRIMARY KEY (id);
 J   ALTER TABLE ONLY public.administrators DROP CONSTRAINT administrators_pk;
       public            postgres    false    216            �           2606    16825 &   airline_companies airline_companies_pk 
   CONSTRAINT     d   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_pk PRIMARY KEY (id);
 P   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_pk;
       public            postgres    false    207            �           2606    16796    countries countries_pk 
   CONSTRAINT     T   ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_pk PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.countries DROP CONSTRAINT countries_pk;
       public            postgres    false    201            �           2606    16838    customers customers_pk 
   CONSTRAINT     T   ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pk PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.customers DROP CONSTRAINT customers_pk;
       public            postgres    false    209            �           2606    16805    flights flights_pk 
   CONSTRAINT     P   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_pk PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_pk;
       public            postgres    false    203            �           2606    16813    tickets tickets_pk 
   CONSTRAINT     P   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_pk PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_pk;
       public            postgres    false    205            �           2606    16868    user_roles user_roles_pk 
   CONSTRAINT     V   ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_pk PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.user_roles DROP CONSTRAINT user_roles_pk;
       public            postgres    false    214            �           2606    16855    users users_pk 
   CONSTRAINT     L   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pk;
       public            postgres    false    212            �           1259    16991    administrators_user_id_uindex    INDEX     b   CREATE UNIQUE INDEX administrators_user_id_uindex ON public.administrators USING btree (user_id);
 1   DROP INDEX public.administrators_user_id_uindex;
       public            postgres    false    216            �           1259    16826    airline_companies_name_uindex    INDEX     b   CREATE UNIQUE INDEX airline_companies_name_uindex ON public.airline_companies USING btree (name);
 1   DROP INDEX public.airline_companies_name_uindex;
       public            postgres    false    207            �           1259    16827     airline_companies_user_id_uindex    INDEX     h   CREATE UNIQUE INDEX airline_companies_user_id_uindex ON public.airline_companies USING btree (user_id);
 4   DROP INDEX public.airline_companies_user_id_uindex;
       public            postgres    false    207            �           1259    16797    countries_name_uindex    INDEX     R   CREATE UNIQUE INDEX countries_name_uindex ON public.countries USING btree (name);
 )   DROP INDEX public.countries_name_uindex;
       public            postgres    false    201            �           1259    16839    customers_credit_card_no_uindex    INDEX     f   CREATE UNIQUE INDEX customers_credit_card_no_uindex ON public.customers USING btree (credit_card_no);
 3   DROP INDEX public.customers_credit_card_no_uindex;
       public            postgres    false    209            �           1259    16840    customers_phone_no_uindex    INDEX     Z   CREATE UNIQUE INDEX customers_phone_no_uindex ON public.customers USING btree (phone_no);
 -   DROP INDEX public.customers_phone_no_uindex;
       public            postgres    false    209            �           1259    16841    customers_user_id_uindex    INDEX     X   CREATE UNIQUE INDEX customers_user_id_uindex ON public.customers USING btree (user_id);
 ,   DROP INDEX public.customers_user_id_uindex;
       public            postgres    false    209            �           1259    17026 $   tickets_flight_id_customer_id_uindex    INDEX     q   CREATE UNIQUE INDEX tickets_flight_id_customer_id_uindex ON public.tickets USING btree (flight_id, customer_id);
 8   DROP INDEX public.tickets_flight_id_customer_id_uindex;
       public            postgres    false    205    205            �           1259    16869    user_roles_role_name_uindex    INDEX     ^   CREATE UNIQUE INDEX user_roles_role_name_uindex ON public.user_roles USING btree (role_name);
 /   DROP INDEX public.user_roles_role_name_uindex;
       public            postgres    false    214            �           1259    16856    users_email_uindex    INDEX     L   CREATE UNIQUE INDEX users_email_uindex ON public.users USING btree (email);
 &   DROP INDEX public.users_email_uindex;
       public            postgres    false    212            �           1259    16857    users_username_uindex    INDEX     R   CREATE UNIQUE INDEX users_username_uindex ON public.users USING btree (username);
 )   DROP INDEX public.users_username_uindex;
       public            postgres    false    212            �           2606    16992 )   administrators administrators_users_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 S   ALTER TABLE ONLY public.administrators DROP CONSTRAINT administrators_users_id_fk;
       public          postgres    false    2972    216    212            �           2606    16907 3   airline_companies airline_companies_countries_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_countries_id_fk FOREIGN KEY (country_id) REFERENCES public.countries(id);
 ]   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_countries_id_fk;
       public          postgres    false    207    2955    201            �           2606    16912 /   airline_companies airline_companies_users_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 Y   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_users_id_fk;
       public          postgres    false    2972    207    212            �           2606    16917    customers customers_users_id_fk    FK CONSTRAINT     ~   ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 I   ALTER TABLE ONLY public.customers DROP CONSTRAINT customers_users_id_fk;
       public          postgres    false    212    209    2972            �           2606    16887 '   flights flights_airline_companies_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_airline_companies_id_fk FOREIGN KEY (airline_company_id) REFERENCES public.airline_companies(id);
 Q   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_airline_companies_id_fk;
       public          postgres    false    207    2963    203            �           2606    16882    flights flights_countries_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk FOREIGN KEY (origin_country_id) REFERENCES public.countries(id);
 I   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_countries_id_fk;
       public          postgres    false    201    2955    203            �           2606    16892 !   flights flights_countries_id_fk_2    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk_2 FOREIGN KEY (destination_country_id) REFERENCES public.countries(id);
 K   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_countries_id_fk_2;
       public          postgres    false    203    2955    201            �           2606    17027    tickets tickets_customers_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_customers_id_fk FOREIGN KEY (customer_id) REFERENCES public.customers(id);
 I   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_customers_id_fk;
       public          postgres    false    205    209    2968            �           2606    17021    tickets tickets_flights_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_flights_id_fk FOREIGN KEY (flight_id) REFERENCES public.flights(id);
 G   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_flights_id_fk;
       public          postgres    false    2957    205    203            �           2606    16922    users users_user_roles_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_user_roles_id_fk FOREIGN KEY (user_role) REFERENCES public.user_roles(id);
 F   ALTER TABLE ONLY public.users DROP CONSTRAINT users_user_roles_id_fk;
       public          postgres    false    2975    212    214            @   >   x��A
�0 ���1Bғ�4�ЦPA��Q֌��W(*��gq�;äqyf�Fw������      7   %   x�3�tI�)I�4�4�2�t��u��4�4����� _�      1   7   x�3��,.JL��2���,IMQ(.I,I-�2�t+J�K��2��,I̩����� YE�      9   X   x��9
� ���)r����!]�B�*��R��������:PҙKnC�#��A	`2���R�b����]���X,<�=pY      3   \   x�]��	� �o3����h��t�9%�H�����I��0>����nla&:e�Z2�ưF���'l�,���)ª<���X&�7�|���"�      5   !   x�3�4�4�2�F\ƜF@�Hr��qqq 4/o      >   :   x�3�tL����,.)J,�/�2�t.-.��M-�2�t�,���KUp��-H̫����� ���      <   �   x�U�K�0Dד�T��g�A�X��H$�j�N�S6�d/��X�-��Ѭ��Q�]�B�5ݗk��5$��7|�8;��w���v&���Z�9�@��@�{�}���p
�v�7Gȓj;"X���<d.}I�'�R��?�6�z��1?W�F�      �    C           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            D           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            E           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            F           1262    16785    flights management    DATABASE     p   CREATE DATABASE "flights management" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Hebrew_Israel.1255';
 $   DROP DATABASE "flights management";
                postgres    false                        2615    2200    public    SCHEMA        CREATE SCHEMA public;
    DROP SCHEMA public;
                postgres    false            G           0    0    SCHEMA public    COMMENT     6   COMMENT ON SCHEMA public IS 'standard public schema';
                   postgres    false    3            �            1255    16954 )   sp_add_admin(text, text, integer, bigint) 	   PROCEDURE       CREATE PROCEDURE public.sp_add_admin(_first_name text, _last_name text, _level integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   insert into administrators (first_name,last_name,"level",user_id) values (_first_name,_last_name,_level,_user_id);
   end;
   $$;
 h   DROP PROCEDURE public.sp_add_admin(_first_name text, _last_name text, _level integer, _user_id bigint);
       public          postgres    false    3            �            1255    17041 $   sp_add_airline(text, bigint, bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_add_airline(_name text, _country_id bigint, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into airline_companies (name,country_id,user_id) values (_name,_country_id,_user_id);
     end;
$$;
 W   DROP PROCEDURE public.sp_add_airline(_name text, _country_id bigint, _user_id bigint);
       public          postgres    false    3            �            1255    16948    sp_add_country(text) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_add_country(_name text)
    LANGUAGE plpgsql
    AS $$
   begin 
	   insert into countries (name) values (_name);
   end
$$;
 2   DROP PROCEDURE public.sp_add_country(_name text);
       public          postgres    false    3            �            1255    17035 5   sp_add_customer(text, text, text, text, text, bigint) 	   PROCEDURE     r  CREATE PROCEDURE public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    insert into customers(first_name,last_name,address,phone_no,credit_card_no,user_id) values (_first_name,_last_name,_address,_phone_no,_credit_card_no,_user_id);
    end;
$$;
 �   DROP PROCEDURE public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint);
       public          postgres    false    3                       1255    17052 h   sp_add_flight(bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer) 	   PROCEDURE     5  CREATE PROCEDURE public.sp_add_flight(_airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into flights (airline_company_id,origin_country_id,destination_country_id,departure_time,landing_time,remaining_tickets)
	     values (_airline_company_id,_origin_country_id,_destination_country_id,_departure_time,_landing_time,_remaining_tickets);
     end;
$$;
 �   DROP PROCEDURE public.sp_add_flight(_airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false    3            �            1255    17017    sp_add_ticket(bigint, bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_add_ticket(_flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into tickets (flight_id,customer_id) values (_flight_id,_customer_id);
     end;
$$;
 M   DROP PROCEDURE public.sp_add_ticket(_flight_id bigint, _customer_id bigint);
       public          postgres    false    3            �            1255    17002 %   sp_add_user(text, text, text, bigint) 	   PROCEDURE       CREATE PROCEDURE public.sp_add_user(_user_name text, _password text, _email text, _user_role bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     insert into users (username,"password",email,user_role) values (_user_name,_password,_email,_user_role);
     end;
$$;
 d   DROP PROCEDURE public.sp_add_user(_user_name text, _password text, _email text, _user_role bigint);
       public          postgres    false    3            �            1255    16956    sp_get_admin_by_id(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_admin_by_id(_id bigint) RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators where administrators.id=_id;
   end;
$$;
 5   DROP FUNCTION public.sp_get_admin_by_id(_id bigint);
       public          postgres    false    3                       1255    17045 !   sp_get_airline_by_country(bigint)    FUNCTION     b  CREATE FUNCTION public.sp_get_airline_by_country(_country_id bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies ac where ac.country_id = (select countries.id from countries where countries.id = _country_id);
     end;
$$;
 D   DROP FUNCTION public.sp_get_airline_by_country(_country_id bigint);
       public          postgres    false    3            �            1255    17040    sp_get_airline_by_id(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_airline_by_id(_id bigint) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies where airline_companies.id=_id;
     end;
$$;
 7   DROP FUNCTION public.sp_get_airline_by_id(_id bigint);
       public          postgres    false    3                       1255    17044 !   sp_get_airline_by_user_name(text)    FUNCTION     U  CREATE FUNCTION public.sp_get_airline_by_user_name(_user_name text) RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies ac where ac.user_id = (select users.id from users where users.username=_user_name);
     end;
$$;
 C   DROP FUNCTION public.sp_get_airline_by_user_name(_user_name text);
       public          postgres    false    3            �            1255    16983    sp_get_all_admins()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_admins() RETURNS TABLE(id bigint, first_name text, last_name text, level integer, user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from administrators;
   end;
$$;
 *   DROP FUNCTION public.sp_get_all_admins();
       public          postgres    false    3                       1255    17046    sp_get_all_airlines()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_airlines() RETURNS TABLE(id bigint, name text, country_id bigint, user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from airline_companies;
     end;
$$;
 ,   DROP FUNCTION public.sp_get_all_airlines();
       public          postgres    false    3            �            1255    16946    sp_get_all_countries()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_countries() RETURNS TABLE(id bigint, name text)
    LANGUAGE plpgsql
    AS $$
  begin
	  return query
	  select * from countries;
  end
$$;
 -   DROP FUNCTION public.sp_get_all_countries();
       public          postgres    false    3            �            1255    17032    sp_get_all_customers()    FUNCTION       CREATE FUNCTION public.sp_get_all_customers() RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers;
    end;
$$;
 -   DROP FUNCTION public.sp_get_all_customers();
       public          postgres    false    3                       1255    17049    sp_get_all_flights()    FUNCTION     w  CREATE FUNCTION public.sp_get_all_flights() RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights;
     end;
$$;
 +   DROP FUNCTION public.sp_get_all_flights();
       public          postgres    false    3            �            1255    17014    sp_get_all_tickets()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_tickets() RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from tickets;
   end;
$$;
 +   DROP FUNCTION public.sp_get_all_tickets();
       public          postgres    false    3            �            1255    16998    sp_get_all_users()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_users() RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users;
    end;
$$;
 )   DROP FUNCTION public.sp_get_all_users();
       public          postgres    false    3                       1255    17064    sp_get_all_vacancy_flights()    FUNCTION     �  CREATE FUNCTION public.sp_get_all_vacancy_flights() RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.remaining_tickets >0;
     end;
$$;
 3   DROP FUNCTION public.sp_get_all_vacancy_flights();
       public          postgres    false    3            �            1255    16947    sp_get_country_by_id(bigint)    FUNCTION     �   CREATE FUNCTION public.sp_get_country_by_id(_id bigint) RETURNS TABLE(id bigint, name text)
    LANGUAGE plpgsql
    AS $$
  begin
	  return query
	  select * from countries where countries.id=_id;
  end
$$;
 7   DROP FUNCTION public.sp_get_country_by_id(_id bigint);
       public          postgres    false    3            �            1255    17033    sp_get_customer_by_id(bigint)    FUNCTION     8  CREATE FUNCTION public.sp_get_customer_by_id(_id bigint) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.id=_id;
    end;
$$;
 8   DROP FUNCTION public.sp_get_customer_by_id(_id bigint);
       public          postgres    false    3            �            1255    17038 "   sp_get_customer_by_user_name(text)    FUNCTION     �  CREATE FUNCTION public.sp_get_customer_by_user_name(_user_name text) RETURNS TABLE(id bigint, first_name text, last_name text, address text, phone_no text, credit_card_no text, user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from customers where customers.user_id=(
select u.id from customers c join users u on c.user_id =u.id where u.username =_user_name);
    end;
$$;
 D   DROP FUNCTION public.sp_get_customer_by_user_name(_user_name text);
       public          postgres    false    3                       1255    17063 !   sp_get_flight_by_customer(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flight_by_customer(_customer_id bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where f.id in (select t.flight_id from tickets t where t.customer_id = _customer_id);
     end;
$$;
 E   DROP FUNCTION public.sp_get_flight_by_customer(_customer_id bigint);
       public          postgres    false    3                       1255    17051    sp_get_flight_by_id(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flight_by_id(_id bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.id=_id;
     end;
$$;
 6   DROP FUNCTION public.sp_get_flight_by_id(_id bigint);
       public          postgres    false    3                       1255    17066 =   sp_get_flights_by_departure_date(timestamp without time zone)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_departure_date(_departure_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.departure_time as date)=cast (_departure_time as date);
     end;
$$;
 d   DROP FUNCTION public.sp_get_flights_by_departure_date(_departure_time timestamp without time zone);
       public          postgres    false    3                       1255    17056 -   sp_get_flights_by_destination_country(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_destination_country(_country_code bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.destination_country_id =_country_code;
     end;
$$;
 R   DROP FUNCTION public.sp_get_flights_by_destination_country(_country_code bigint);
       public          postgres    false    3                       1255    17068 ;   sp_get_flights_by_landing_date(timestamp without time zone)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_landing_date(_landing_time timestamp without time zone) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights f where cast (f.landing_time as date) =cast (_landing_time as date);
     end;
$$;
 `   DROP FUNCTION public.sp_get_flights_by_landing_date(_landing_time timestamp without time zone);
       public          postgres    false    3            
           1255    17055 (   sp_get_flights_by_origin_country(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flights_by_origin_country(_country_code bigint) RETURNS TABLE(id bigint, airline_company_id bigint, origin_country_id bigint, destination_country_id bigint, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
     begin
	     return query
	     select * from flights where flights.origin_country_id =_country_code;
     end;
$$;
 M   DROP FUNCTION public.sp_get_flights_by_origin_country(_country_code bigint);
       public          postgres    false    3            �            1255    17016    sp_get_ticket_by_id(bigint)    FUNCTION     �   CREATE FUNCTION public.sp_get_ticket_by_id(_id bigint) RETURNS TABLE(id bigint, flight_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin
	   return query
	   select * from tickets where tickets.id=_id;
   end;
$$;
 6   DROP FUNCTION public.sp_get_ticket_by_id(_id bigint);
       public          postgres    false    3            �            1255    16999    sp_get_user_by_id(bigint)    FUNCTION     	  CREATE FUNCTION public.sp_get_user_by_id(_id bigint) RETURNS TABLE(id bigint, user_name text, password text, email text, user_role integer)
    LANGUAGE plpgsql
    AS $$
    begin 
	    return query
	    select * from users where users.id = _id;
    end;
$$;
 4   DROP FUNCTION public.sp_get_user_by_id(_id bigint);
       public          postgres    false    3            �            1255    16997    sp_remove_admin(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_admin(_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   delete from administrators where id=_id;
   end;
$$;
 3   DROP PROCEDURE public.sp_remove_admin(_id bigint);
       public          postgres    false    3                        1255    17043    sp_remove_airline(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_airline(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from airline_companies where id=_id;
     end;
$$;
 5   DROP PROCEDURE public.sp_remove_airline(_id bigint);
       public          postgres    false    3            �            1255    16951    sp_remove_country(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_country(_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   delete from countries where id=_id;
   end
$$;
 5   DROP PROCEDURE public.sp_remove_country(_id bigint);
       public          postgres    false    3            �            1255    17037    sp_remove_customer(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_customer(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from customers where id=_id;
     end;
$$;
 6   DROP PROCEDURE public.sp_remove_customer(_id bigint);
       public          postgres    false    3            	           1255    17054    sp_remove_flight(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_flight(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from flights where id=_id;
     end;
$$;
 4   DROP PROCEDURE public.sp_remove_flight(_id bigint);
       public          postgres    false    3            �            1255    17019    sp_remove_ticket(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_ticket(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from tickets where id=_id;
     end;
$$;
 4   DROP PROCEDURE public.sp_remove_ticket(_id bigint);
       public          postgres    false    3            �            1255    17004    sp_remove_user(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_user(_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     delete from users where id = _id;
     end;
$$;
 2   DROP PROCEDURE public.sp_remove_user(_id bigint);
       public          postgres    false    3            �            1255    16990 4   sp_update_admin(bigint, text, text, integer, bigint) 	   PROCEDURE     3  CREATE PROCEDURE public.sp_update_admin(_id bigint, _first_name text, _last_name text, _level integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
   begin 
	   update administrators set first_name = _first_name, last_name = _last_name, "level" =_level, user_id = _user_id where id=_id;
   end;
$$;
 w   DROP PROCEDURE public.sp_update_admin(_id bigint, _first_name text, _last_name text, _level integer, _user_id bigint);
       public          postgres    false    3                       1255    17047 /   sp_update_airline(bigint, text, bigint, bigint) 	   PROCEDURE       CREATE PROCEDURE public.sp_update_airline(_id bigint, _name text, _country_id bigint, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update airline_companies set "name" =_name,country_id =_country_id,user_id =_user_id where id=_id;
     end;
$$;
 f   DROP PROCEDURE public.sp_update_airline(_id bigint, _name text, _country_id bigint, _user_id bigint);
       public          postgres    false    3            �            1255    16949    sp_update_country(bigint, text) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_update_country(_id bigint, _name text)
    LANGUAGE plpgsql
    AS $$
   begin 
	   update countries set "name" =_name where id=_id;
   end
$$;
 A   DROP PROCEDURE public.sp_update_country(_id bigint, _name text);
       public          postgres    false    3            �            1255    17036 @   sp_update_customer(bigint, text, text, text, text, text, bigint) 	   PROCEDURE     �  CREATE PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
    begin 
	    update customers set first_name =_first_name,last_name =_last_name,address =_address,phone_no =_phone_no,credit_card_no =_credit_card_no,user_id =_user_id where id=_id;
    end;
$$;
 �   DROP PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_no text, _credit_card_no text, _user_id bigint);
       public          postgres    false    3                       1255    17053 s   sp_update_flight(bigint, bigint, bigint, bigint, timestamp without time zone, timestamp without time zone, integer) 	   PROCEDURE     Q  CREATE PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
      begin 
	      update flights set airline_company_id =_airline_company_id,origin_country_id =_origin_country_id,destination_country_id =_destination_country_id,
	      departure_time = _departure_time,landing_time =_landing_time,remaining_tickets =_remaining_tickets where id=_id;
      end;
$$;
 �   DROP PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id bigint, _destination_country_id bigint, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false    3            �            1255    17018 (   sp_update_ticket(bigint, bigint, bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update tickets set flight_id = _flight_id, customer_id = _customer_id where id=_id;
     end;
$$;
 \   DROP PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint);
       public          postgres    false    3            �            1255    17006 1   sp_update_user(bigint, text, text, text, integer) 	   PROCEDURE     .  CREATE PROCEDURE public.sp_update_user(_id bigint, _user_name text, _password text, _email text, _user_role integer)
    LANGUAGE plpgsql
    AS $$
     begin 
	     update users set username = _user_name, "password" =_password, email = _email, user_role = _user_role where id = _id;
     end;
$$;
 t   DROP PROCEDURE public.sp_update_user(_id bigint, _user_name text, _password text, _email text, _user_role integer);
       public          postgres    false    3            �            1259    16872    administrators    TABLE     �   CREATE TABLE public.administrators (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    level integer NOT NULL,
    user_id bigint NOT NULL
);
 "   DROP TABLE public.administrators;
       public         heap    postgres    false    3            �            1259    16870    administrators_id_seq    SEQUENCE     �   CREATE SEQUENCE public.administrators_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.administrators_id_seq;
       public          postgres    false    216    3            H           0    0    administrators_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.administrators_id_seq OWNED BY public.administrators.id;
          public          postgres    false    215            �            1259    16817    airline_companies    TABLE     �   CREATE TABLE public.airline_companies (
    id bigint NOT NULL,
    name text NOT NULL,
    country_id bigint NOT NULL,
    user_id bigint NOT NULL
);
 %   DROP TABLE public.airline_companies;
       public         heap    postgres    false    3            �            1259    16815    airline_companies_id_seq    SEQUENCE     �   CREATE SEQUENCE public.airline_companies_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.airline_companies_id_seq;
       public          postgres    false    207    3            I           0    0    airline_companies_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.airline_companies_id_seq OWNED BY public.airline_companies.id;
          public          postgres    false    206            �            1259    16788 	   countries    TABLE     R   CREATE TABLE public.countries (
    id bigint NOT NULL,
    name text NOT NULL
);
    DROP TABLE public.countries;
       public         heap    postgres    false    3            �            1259    16786    countries_id_seq    SEQUENCE     y   CREATE SEQUENCE public.countries_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.countries_id_seq;
       public          postgres    false    201    3            J           0    0    countries_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.countries_id_seq OWNED BY public.countries.id;
          public          postgres    false    200            �            1259    16830 	   customers    TABLE     �   CREATE TABLE public.customers (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    address text NOT NULL,
    phone_no text NOT NULL,
    credit_card_no text NOT NULL,
    user_id bigint NOT NULL
);
    DROP TABLE public.customers;
       public         heap    postgres    false    3            �            1259    16828    customers_id_seq    SEQUENCE     y   CREATE SEQUENCE public.customers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.customers_id_seq;
       public          postgres    false    209    3            K           0    0    customers_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.customers_id_seq OWNED BY public.customers.id;
          public          postgres    false    208            �            1259    16800    flights    TABLE     Q  CREATE TABLE public.flights (
    id bigint NOT NULL,
    airline_company_id bigint NOT NULL,
    origin_country_id bigint NOT NULL,
    destination_country_id bigint NOT NULL,
    departure_time timestamp(0) without time zone NOT NULL,
    landing_time timestamp(0) without time zone NOT NULL,
    remaining_tickets integer NOT NULL
);
    DROP TABLE public.flights;
       public         heap    postgres    false    3            �            1259    16798    flights_id_seq    SEQUENCE     w   CREATE SEQUENCE public.flights_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.flights_id_seq;
       public          postgres    false    203    3            L           0    0    flights_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.flights_id_seq OWNED BY public.flights.id;
          public          postgres    false    202            �            1259    16808    tickets    TABLE     x   CREATE TABLE public.tickets (
    id bigint NOT NULL,
    flight_id bigint NOT NULL,
    customer_id bigint NOT NULL
);
    DROP TABLE public.tickets;
       public         heap    postgres    false    3            �            1259    16806    tickets_id_seq    SEQUENCE     w   CREATE SEQUENCE public.tickets_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.tickets_id_seq;
       public          postgres    false    205    3            M           0    0    tickets_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.tickets_id_seq OWNED BY public.tickets.id;
          public          postgres    false    204            �            1259    16860 
   user_roles    TABLE     Y   CREATE TABLE public.user_roles (
    id integer NOT NULL,
    role_name text NOT NULL
);
    DROP TABLE public.user_roles;
       public         heap    postgres    false    3            �            1259    16858    user_roles_id_seq    SEQUENCE     �   CREATE SEQUENCE public.user_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.user_roles_id_seq;
       public          postgres    false    3    214            N           0    0    user_roles_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.user_roles_id_seq OWNED BY public.user_roles.id;
          public          postgres    false    213            �            1259    16846    users    TABLE     �   CREATE TABLE public.users (
    id bigint NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    email text NOT NULL,
    user_role integer NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false    3            �            1259    16842    users_id_seq    SEQUENCE     u   CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    212    3            O           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    210            �            1259    16844    users_user_role_seq    SEQUENCE     �   CREATE SEQUENCE public.users_user_role_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.users_user_role_seq;
       public          postgres    false    3    212            P           0    0    users_user_role_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.users_user_role_seq OWNED BY public.users.user_role;
          public          postgres    false    211            �           2604    16957    administrators id    DEFAULT     v   ALTER TABLE ONLY public.administrators ALTER COLUMN id SET DEFAULT nextval('public.administrators_id_seq'::regclass);
 @   ALTER TABLE public.administrators ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    216    216            �           2604    16820    airline_companies id    DEFAULT     |   ALTER TABLE ONLY public.airline_companies ALTER COLUMN id SET DEFAULT nextval('public.airline_companies_id_seq'::regclass);
 C   ALTER TABLE public.airline_companies ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    206    207    207            �           2604    16791    countries id    DEFAULT     l   ALTER TABLE ONLY public.countries ALTER COLUMN id SET DEFAULT nextval('public.countries_id_seq'::regclass);
 ;   ALTER TABLE public.countries ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    201    200    201            �           2604    16833    customers id    DEFAULT     l   ALTER TABLE ONLY public.customers ALTER COLUMN id SET DEFAULT nextval('public.customers_id_seq'::regclass);
 ;   ALTER TABLE public.customers ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    208    209    209            �           2604    16803 
   flights id    DEFAULT     h   ALTER TABLE ONLY public.flights ALTER COLUMN id SET DEFAULT nextval('public.flights_id_seq'::regclass);
 9   ALTER TABLE public.flights ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    202    203    203            �           2604    16811 
   tickets id    DEFAULT     h   ALTER TABLE ONLY public.tickets ALTER COLUMN id SET DEFAULT nextval('public.tickets_id_seq'::regclass);
 9   ALTER TABLE public.tickets ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    205    204    205            �           2604    16863    user_roles id    DEFAULT     n   ALTER TABLE ONLY public.user_roles ALTER COLUMN id SET DEFAULT nextval('public.user_roles_id_seq'::regclass);
 <   ALTER TABLE public.user_roles ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    214    213    214            �           2604    16849    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    210    212    212            �           2604    16850    users user_role    DEFAULT     r   ALTER TABLE ONLY public.users ALTER COLUMN user_role SET DEFAULT nextval('public.users_user_role_seq'::regclass);
 >   ALTER TABLE public.users ALTER COLUMN user_role DROP DEFAULT;
       public          postgres    false    212    211    212            @          0    16872    administrators 
   TABLE DATA           S   COPY public.administrators (id, first_name, last_name, level, user_id) FROM stdin;
    public          postgres    false    216   �        7          0    16817    airline_companies 
   TABLE DATA           J   COPY public.airline_companies (id, name, country_id, user_id) FROM stdin;
    public          postgres    false    207   H        1          0    16788 	   countries 
   TABLE DATA           -   COPY public.countries (id, name) FROM stdin;
    public          postgres    false    201   /        9          0    16830 	   customers 
   TABLE DATA           j   COPY public.customers (id, first_name, last_name, address, phone_no, credit_card_no, user_id) FROM stdin;
    public          postgres    false    209   A        3          0    16800    flights 
   TABLE DATA           �   COPY public.flights (id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) FROM stdin;
    public          postgres    false    203   b        5          0    16808    tickets 
   TABLE DATA           =   COPY public.tickets (id, flight_id, customer_id) FROM stdin;
    public          postgres    false    205   f        >          0    16860 
   user_roles 
   TABLE DATA           3   COPY public.user_roles (id, role_name) FROM stdin;
    public          postgres    false    214   +        <          0    16846    users 
   TABLE DATA           I   COPY public.users (id, username, password, email, user_role) FROM stdin;
    public          postgres    false    212   D        Q           0    0    administrators_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.administrators_id_seq', 4, true);
          public          postgres    false    215            R           0    0    airline_companies_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.airline_companies_id_seq', 5, true);
          public          postgres    false    206            S           0    0    countries_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.countries_id_seq', 6, true);
          public          postgres    false    200            T           0    0    customers_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.customers_id_seq', 4, true);
          public          postgres    false    208            U           0    0    flights_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.flights_id_seq', 8, true);
          public          postgres    false    202            V           0    0    tickets_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.tickets_id_seq', 5, true);
          public          postgres    false    204            W           0    0    user_roles_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.user_roles_id_seq', 4, true);
          public          postgres    false    213            X           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 10, true);
          public          postgres    false    210            Y           0    0    users_user_role_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.users_user_role_seq', 1, false);
          public          postgres    false    211            �           2606    16959     administrators administrators_pk 
   CONSTRAINT     ^   ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_pk PRIMARY KEY (id);
 J   ALTER TABLE ONLY public.administrators DROP CONSTRAINT administrators_pk;
       public            postgres    false    216            �           2606    16825 &   airline_companies airline_companies_pk 
   CONSTRAINT     d   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_pk PRIMARY KEY (id);
 P   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_pk;
       public            postgres    false    207            �           2606    16796    countries countries_pk 
   CONSTRAINT     T   ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_pk PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.countries DROP CONSTRAINT countries_pk;
       public            postgres    false    201            �           2606    16838    customers customers_pk 
   CONSTRAINT     T   ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pk PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.customers DROP CONSTRAINT customers_pk;
       public            postgres    false    209            �           2606    16805    flights flights_pk 
   CONSTRAINT     P   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_pk PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_pk;
       public            postgres    false    203            �           2606    16813    tickets tickets_pk 
   CONSTRAINT     P   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_pk PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_pk;
       public            postgres    false    205            �           2606    16868    user_roles user_roles_pk 
   CONSTRAINT     V   ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_pk PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.user_roles DROP CONSTRAINT user_roles_pk;
       public            postgres    false    214            �           2606    16855    users users_pk 
   CONSTRAINT     L   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pk;
       public            postgres    false    212            �           1259    16991    administrators_user_id_uindex    INDEX     b   CREATE UNIQUE INDEX administrators_user_id_uindex ON public.administrators USING btree (user_id);
 1   DROP INDEX public.administrators_user_id_uindex;
       public            postgres    false    216            �           1259    16826    airline_companies_name_uindex    INDEX     b   CREATE UNIQUE INDEX airline_companies_name_uindex ON public.airline_companies USING btree (name);
 1   DROP INDEX public.airline_companies_name_uindex;
       public            postgres    false    207            �           1259    16827     airline_companies_user_id_uindex    INDEX     h   CREATE UNIQUE INDEX airline_companies_user_id_uindex ON public.airline_companies USING btree (user_id);
 4   DROP INDEX public.airline_companies_user_id_uindex;
       public            postgres    false    207            �           1259    16797    countries_name_uindex    INDEX     R   CREATE UNIQUE INDEX countries_name_uindex ON public.countries USING btree (name);
 )   DROP INDEX public.countries_name_uindex;
       public            postgres    false    201            �           1259    16839    customers_credit_card_no_uindex    INDEX     f   CREATE UNIQUE INDEX customers_credit_card_no_uindex ON public.customers USING btree (credit_card_no);
 3   DROP INDEX public.customers_credit_card_no_uindex;
       public            postgres    false    209            �           1259    16840    customers_phone_no_uindex    INDEX     Z   CREATE UNIQUE INDEX customers_phone_no_uindex ON public.customers USING btree (phone_no);
 -   DROP INDEX public.customers_phone_no_uindex;
       public            postgres    false    209            �           1259    16841    customers_user_id_uindex    INDEX     X   CREATE UNIQUE INDEX customers_user_id_uindex ON public.customers USING btree (user_id);
 ,   DROP INDEX public.customers_user_id_uindex;
       public            postgres    false    209            �           1259    17026 $   tickets_flight_id_customer_id_uindex    INDEX     q   CREATE UNIQUE INDEX tickets_flight_id_customer_id_uindex ON public.tickets USING btree (flight_id, customer_id);
 8   DROP INDEX public.tickets_flight_id_customer_id_uindex;
       public            postgres    false    205    205            �           1259    16869    user_roles_role_name_uindex    INDEX     ^   CREATE UNIQUE INDEX user_roles_role_name_uindex ON public.user_roles USING btree (role_name);
 /   DROP INDEX public.user_roles_role_name_uindex;
       public            postgres    false    214            �           1259    16856    users_email_uindex    INDEX     L   CREATE UNIQUE INDEX users_email_uindex ON public.users USING btree (email);
 &   DROP INDEX public.users_email_uindex;
       public            postgres    false    212            �           1259    16857    users_username_uindex    INDEX     R   CREATE UNIQUE INDEX users_username_uindex ON public.users USING btree (username);
 )   DROP INDEX public.users_username_uindex;
       public            postgres    false    212            �           2606    16992 )   administrators administrators_users_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 S   ALTER TABLE ONLY public.administrators DROP CONSTRAINT administrators_users_id_fk;
       public          postgres    false    2972    216    212            �           2606    16907 3   airline_companies airline_companies_countries_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_countries_id_fk FOREIGN KEY (country_id) REFERENCES public.countries(id);
 ]   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_countries_id_fk;
       public          postgres    false    207    2955    201            �           2606    16912 /   airline_companies airline_companies_users_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 Y   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_users_id_fk;
       public          postgres    false    2972    207    212            �           2606    16917    customers customers_users_id_fk    FK CONSTRAINT     ~   ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 I   ALTER TABLE ONLY public.customers DROP CONSTRAINT customers_users_id_fk;
       public          postgres    false    212    209    2972            �           2606    16887 '   flights flights_airline_companies_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_airline_companies_id_fk FOREIGN KEY (airline_company_id) REFERENCES public.airline_companies(id);
 Q   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_airline_companies_id_fk;
       public          postgres    false    207    2963    203            �           2606    16882    flights flights_countries_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk FOREIGN KEY (origin_country_id) REFERENCES public.countries(id);
 I   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_countries_id_fk;
       public          postgres    false    201    2955    203            �           2606    16892 !   flights flights_countries_id_fk_2    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk_2 FOREIGN KEY (destination_country_id) REFERENCES public.countries(id);
 K   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_countries_id_fk_2;
       public          postgres    false    203    2955    201            �           2606    17027    tickets tickets_customers_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_customers_id_fk FOREIGN KEY (customer_id) REFERENCES public.customers(id);
 I   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_customers_id_fk;
       public          postgres    false    205    209    2968            �           2606    17021    tickets tickets_flights_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_flights_id_fk FOREIGN KEY (flight_id) REFERENCES public.flights(id);
 G   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_flights_id_fk;
       public          postgres    false    2957    205    203            �           2606    16922    users users_user_roles_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_user_roles_id_fk FOREIGN KEY (user_role) REFERENCES public.user_roles(id);
 F   ALTER TABLE ONLY public.users DROP CONSTRAINT users_user_roles_id_fk;
       public          postgres    false    2975    212    214           