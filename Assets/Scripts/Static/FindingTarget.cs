using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindingTarget
{
    public static Unit CheckForNearestEnemyUnit(Vector3 origin, float range, LayerMask layerMask, string tag)
    {
        RaycastHit[] hits = Physics.SphereCastAll(origin,
                                                    range,
                                                    Vector3.up,
                                                    layerMask);

        GameObject closest = null;
        float closestDist = 0f;

        for (int x = 0; x < hits.Length; x++)
        {
            // skip if this is not a target's tag
            if (hits[x].collider.tag != tag)
                continue;

            Unit target = hits[x].collider.GetComponent<Unit>();
            float dist = Vector3.Distance(origin, hits[x].transform.position);

            // skip if this is not a unit
            if (target == null)
                continue;

            // skip if it is any dead unit
            if (target.HP <= 0)
                continue;
            // if the closest is null or the distance is less than the closest distance it currently has
            else if ((closest == null) || (dist < closestDist))
            {
                closest = hits[x].collider.gameObject;
                closestDist = dist;
            }
        }

        if (closest != null)
            return closest.GetComponent<Unit>();
        else
            return null;
    }

    // checks for nearest enemy building with a sphere cast
    public static Building CheckForNearestEnemyBuilding(Vector3 origin, float range, LayerMask layerMask, string tag){
        RaycastHit[] hits = Physics.SphereCastAll(origin,
                                                    range,
                                                    Vector3.up,
                                                    layerMask);

        GameObject closest = null;
        float closestDist = 0f;

        for (int x = 0; x < hits.Length; x++)
        {
            // skip if this is not a target's tag
            if (hits[x].collider.tag != tag)
                continue;

            //Debug.Log("Test - " + hits[x].collider.gameObject.ToString());
            Building target = hits[x].collider.GetComponent<Building>();
            float dist = Vector3.Distance(origin, hits[x].transform.position);

            // skip if this is not a building
            if (target == null)
                continue;

            // skip if it is any destroyed building
            if (target.HP <= 0)
                continue;
            // if the closest is null or the distance is less than the closest distance it currently has
            else if ((closest == null) || (dist < closestDist))
            {
                closest = hits[x].collider.gameObject;
                closestDist = dist;
            }
        }

        if (closest != null)
            return closest.GetComponent<Building>();
        else
            return null;
    }

    public static GameObject CheckForNearestMine(Vector3 origin, float range, string tag){
        RaycastHit[] hits = Physics.SphereCastAll(origin,
                                                    range,
                                                    Vector3.up, LayerMask.GetMask("Mine"));

        GameObject closest = null;
        float closestDist = 0f;

        for (int x = 0; x < hits.Length; x++)
        {
            // skip if this is not a target's tag
            if (hits[x].collider.tag != tag)
                continue;

            //Debug.Log("Test - " + hits[x].collider.gameObject.ToString());
            Building target = hits[x].collider.GetComponent<Building>();
            float dist = Vector3.Distance(origin, hits[x].transform.position);

            // skip if this is not a building
            if (target == null)
                continue;

            // skip if it is any destroyed building
            if (target.HP <= 0)
                continue;
            // if the closest is null or the distance is less than the closest distance it currently has
            else if ((closest == null) || (dist < closestDist))
            {
                closest = hits[x].collider.gameObject;
                closestDist = dist;
            }
        }

        if (closest != null)
            return closest.gameObject;
        else
            return null;
    }
}
