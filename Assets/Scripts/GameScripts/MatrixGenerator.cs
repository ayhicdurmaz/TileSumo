using UnityEngine;

public class MatrixGenerator : MonoBehaviour
{
    GameManager gm;
    private int[,] matrix;
    public int depth = 5;

    public void FullMatrixInit(int _min ,int _max){
        gm = this.GetComponent<GameManager>();
        matrix = new int[gm.desiredNumTileCount, depth];
        for(int i = 0; i < matrix.GetLength(0); i++){
            for(int j = 0; j < matrix.GetLength(1); j++){
                int rndm = Random.Range(_min, _max);
                matrix[i,j] = Random.Range(_min, _max);
            }
        }
    }

    public int SetTileNumber(int index){
        int number = matrix[index, 0];
        gm.isGameOver = !ControlIfSumPossible(gm.sum);   
        return number;
    }

    public bool ControlIfSumPossible(int _sum){
        bool isPossible = true;
        for(int i = 0; i < matrix.GetLength(0); i++){
            if(matrix[i,0] > _sum){
                isPossible = false;
            }
            else{
                isPossible = true;
                break;
            }
        }
        return isPossible;
    }

    public void UpdateMatrix(int index, int _min, int _max){
        for(int i = 0; i < depth ; i++){
            if(i == depth - 1){
                matrix[index, i] = Random.Range(_min, _max);
            }else{

                matrix[index, i] = matrix[index, i+1]; 
            }
        }
    }

    public int GetMainNumber(int iterationValue){
        int sum = 0; 
        int rndm = 0;
        int[] indexHolder = new int[gm.desiredNumTileCount];
        for(int i = 0; i < iterationValue; i++){
            rndm = Random.Range(0,gm.desiredNumTileCount-1);
            sum += matrix[rndm, indexHolder[rndm]];
            indexHolder[rndm]++;
        }
        return sum;
    }
}
