CREATE OR REPLACE FUNCTION get_visitors(
    _house_mapping_id INT DEFAULT -1,
    _search TEXT DEFAULT '',
    _date_range TEXT DEFAULT 'all',
    _from_date DATE DEFAULT NULL,
    _to_date DATE DEFAULT NULL,
    _checkout_status TEXT DEFAULT 'all',
    _visit_purpose_id INT DEFAULT -1,
    _visitor_status INT DEFAULT -1,
    _sort_column TEXT DEFAULT 'id',
    _sort_order TEXT DEFAULT 'asc'
)
RETURNS TABLE (
    house_name character varying(15),
    house_mapping_id INT,
    id INT,
    name character varying(100),
    phone character varying,
    visit_purpose character varying,
    no_of_visitors INT,
    created_at TIMESTAMP,
    vehicle_no character varying(15),
    approval_status character varying(100),
    check_in_time TIMESTAMP,
    check_out_time TIMESTAMP
) AS $$
DECLARE
    _from_final DATE;
    _to_final DATE;
    _sql TEXT;
BEGIN
    -- Date Range Logic
    _date_range = lower(_date_range);

    IF _date_range = 'today' THEN
        _from_final := CURRENT_DATE;
        _to_final := CURRENT_DATE;
    ELSIF _date_range = 'last7days' THEN
        _from_final := CURRENT_DATE - INTERVAL '7 days';
        _to_final := CURRENT_DATE;
    ELSIF _date_range = 'last30days' THEN
        _from_final := CURRENT_DATE - INTERVAL '30 days';
        _to_final := CURRENT_DATE;
    ELSIF _date_range = 'currentmonth' THEN
        _from_final := date_trunc('month', CURRENT_DATE);
        _to_final := (date_trunc('month', CURRENT_DATE) + INTERVAL '1 month - 1 day')::DATE;
    ELSIF _date_range = 'customdate' THEN
        _from_final := _from_date;
        _to_final := _to_date;
    END IF;

    -- Dynamic SQL Assembly
    _sql := 'SELECT
                hm.house_name,
                v.house_mapping_id as house_id,
                v.id, 
                v.name, 
                v.phone, 
                CASE 
                    WHEN vp.name = ''Other'' 
                        THEN v.visit_purpose_reason 
                    ELSE vp.name 
                    END 
                AS visit_purpose, 
                v.no_of_visitors, 
                v.created_at, 
                v.vehicle_no, 
                vs.name AS approval_status, 
                v.check_in_time, 
                v.check_out_time
            FROM "Visitors" v
            JOIN "VisitPurpose" vp ON v.visit_purpose_id = vp.id
            JOIN "VisitorStatus" vs ON v.status_id = vs.id
            JOIN "HouseMapping" hm on v.house_mapping_id = hm.id
            WHERE v.deleted_by IS NULL';

    -- User-based filter
    IF _house_mapping_id != -1 THEN
        _sql := _sql || ' AND v.house_mapping_id = ' || _house_mapping_id;
    END IF;

    -- Search filter
    IF trim(_search) <> '' THEN
        _sql := _sql || ' AND lower(replace(v.name, '' '', '''')) LIKE ''%' || replace(lower(_search), '''', '''''') || '%''';
    END IF;

    -- Date filter
    IF _date_range <> 'all' AND _from_final IS NOT NULL AND _to_final IS NOT NULL THEN
        _sql := _sql || ' AND v.created_at::DATE BETWEEN ''' || _from_final || ''' AND ''' || _to_final || '''';
    END IF;

    -- Visit Purpose filter
    IF _visit_purpose_id != -1 THEN
        _sql := _sql || ' AND v.visit_purpose_id = ' || _visit_purpose_id;
    END IF;

    -- Visitor Status filter
    IF _visitor_status != -1 THEN
        _sql := _sql || ' AND v.status_id = ' || _visitor_status;
    END IF;

    -- Check Out Status filter
    _checkout_status = lower(_checkout_status);

    IF _checkout_status = 'current' THEN
        _sql := _sql || ' AND v.check_in_time IS NOT NULL AND v.check_out_time IS NULL';
    ELSIF _checkout_status = 'checked out' THEN
        _sql := _sql || ' AND v.check_in_time IS NOT NULL AND v.check_out_time IS NOT NULL';
    END IF;

    -- Sorting
    CASE lower(_sort_column)
        WHEN 'house' THEN _sort_column := 'v.house_mapping_id';
        WHEN 'name' THEN _sort_column := 'v.name';
        WHEN 'phone' THEN _sort_column := 'v.phone';
        WHEN 'purpose' THEN _sort_column := 'vp.name';
        WHEN 'noofvisitors' THEN _sort_column := 'v.no_of_visitors';
        WHEN 'vehicle' THEN _sort_column := 'v.vehicle_no';
        WHEN 'checkin' THEN _sort_column := 'v.check_in_time';
        WHEN 'checkout' THEN _sort_column := 'v.check_out_time';
        ELSE _sort_column := 'v.id';
    END CASE;

    IF lower(_sort_order) NOT IN ('asc', 'desc') THEN
        _sort_order := 'asc';
    END IF;

    -- Final ORDER BY
    _sql := _sql || ' ORDER BY ' || _sort_column || ' ' || _sort_order;

    -- Execute dynamic SQL safely
    RETURN QUERY EXECUTE _sql;

END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_visitors();