using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(LineRenderer),typeof(EdgeCollider2D))]
public class GraphBehaviour : MonoBehaviour {

    public Expression equation;

    private LineRenderer m_lineRenderer;
    private EdgeCollider2D m_collider;

    void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_collider = GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        m_lineRenderer.startWidth = 0.09f;
        m_lineRenderer.endWidth = 0.09f;
        m_lineRenderer.widthMultiplier = 1;
    }

    /// <summary>
    /// Draws the graphic for the passed expression, between the given x limitis, with passed number of points.
    /// Caches the expression.
    /// </summary>
    /// <param name="equation">Expression to use</param>
    /// <param name="startX">Start x at this value</param>
    /// <param name="endX">End x at this value</param>
    /// <param name="numOfIterations">Number of points to use in the graph</param>
    public void Graph(Graph graph)
    {
        Graph(graph.expression, graph.startX, graph.endX);
    }

    /// <summary>
    /// Draws the graphic for the passed expression, between the given x limitis, with passed number of points.
    /// Caches the expression.
    /// </summary>
    /// <param name="equation">Expression to use</param>
    /// <param name="startX">Start x at this value</param>
    /// <param name="endX">End x at this value</param>
    /// <param name="numOfIterations">Number of points to use in the graph</param>
    public void Graph(Expression equation,float startX, float endX, int numOfIterations = 1000)
    {
        this.equation = equation;
        Graph(startX, endX, numOfIterations);
    }

    /// <summary>
    /// Draws the graphic for the cached expression, between the given x limitis, with passed number of points.
    /// Caches the expression.
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="endX"></param>
    /// <param name="numOfIterations"></param>
    public void Graph(float startX, float endX, int numOfIterations = 1000)
    {
        Vector3[] points3 = new Vector3[numOfIterations];
        Vector2[] points2 = new Vector2[numOfIterations];
        float x,y, lerp;
        lerp = 1f / numOfIterations;
        for (int i = 0; i < numOfIterations; i++)
        {
            x = Mathf.Lerp(startX, endX, (i * lerp));
            y = equation.solveForX(x);
            points3[i] = new Vector3(x, y);
            points2[i] = new Vector2(x, y);
        }

        m_lineRenderer.numPositions = numOfIterations;
        m_lineRenderer.SetPositions(points3);
        m_collider.points = points2;
    }

    public void Graph(float startX, float endX, Func<float,float> numGen, int numOfIterations = 1000)
    {
        Vector3[] points3 = new Vector3[numOfIterations];
        Vector2[] points2 = new Vector2[numOfIterations];
        float x, y, lerp;
        lerp = 1f / numOfIterations;
        for (int i = 0; i < numOfIterations; i++)
        {
            x = Mathf.Lerp(startX, endX, (i * lerp));
            y = numGen(x);
            points3[i] = new Vector3(x, y);
            points2[i] = new Vector2(x, y);
        }

        m_lineRenderer.numPositions = numOfIterations;
        m_lineRenderer.SetPositions(points3);
        m_collider.points = points2;
    }
}
