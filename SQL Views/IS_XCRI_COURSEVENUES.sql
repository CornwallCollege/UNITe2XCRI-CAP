CREATE OR REPLACE FORCE VIEW "UNITE"."IS_XCRI_COURSEVENUES" ("CourseOfferingID", "description", "identifier", "title", "address", "postcode", "phone")
                       AS
  SELECT t1.m_id       AS "CourseOfferingID",
    NULL               AS "description",
    t2.s_reference     AS "identifier",
    t2.s_name          AS "title",
    t2.s_siteaddress   AS "address",
    t2.s_sitepostcode  AS "postcode",
    t2.s_sitetelephone AS "phone"
  FROM capd_module t4,
    capd_moduletemplate t3,
    capd_module t1,
    capd_site t2,
    capd_session t6,
    caps_settings t5
  WHERE (t1.m_modulesite   =t2.s_id)
  AND (t1.m_parent         =t3.mt_id)
  AND (t3.mt_templatemodule=t4.m_id)
  AND (t5.set_key          ='System\Institution\CurrentAcademicYear\Start')
  AND (t6.s_start         >=t5.set_value)
  AND (t1.m_modulesession  =t6.s_id);