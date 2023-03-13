
using UnityEditor;


//True will affect child classes of interactable 
[CustomEditor(typeof(Hittable), true)]
public class HittableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Hittable damage = target as Hittable;

        if (target.GetType() == typeof(EventOnlyHittable))
        {
            
            EditorGUILayout.HelpBox("EventOnlyDamage can only use unityevents.", MessageType.Info);
            if (damage.GetComponent<HitEvent>() == null)
            {
                damage.useEvents = true;
                damage.gameObject.AddComponent<HitEvent>();
            }

            return;
        }


        base.OnInspectorGUI();

        if (damage.useEvents)
        {
            if (damage.GetComponent<HitEvent>() == null)
                damage.gameObject.AddComponent<HitEvent>();
        }
        else
        {
            if (damage.GetComponent<HitEvent>() != null)
                DestroyImmediate(damage.GetComponent<HitEvent>());
        }
    }
}
