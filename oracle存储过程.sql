create or replace procedure InsertPoly(id in character,sgid in character,name in nvarchar2,type in nvarchar2,geom in clob,fl in nvarchar2) is

begin

insert into geom_polygon values(id,sgid,name,type,sdo_geometry(geom,4326),fl);
end InsertPoly;