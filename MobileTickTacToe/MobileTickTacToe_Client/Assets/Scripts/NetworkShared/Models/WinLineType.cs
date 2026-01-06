

namespace NetworkShared.Models
{
    public enum WinLineType
    {
        None,           // 아직 승자가 없거나 무승부인 상태
        Diagonal,       // 왼쪽 위(0,0)에서 오른쪽 아래(2,2)로 이어지는 선
        AntiDiagonal,   // 오른쪽 위(0,2)에서 왼쪽 아래(2,0)으로 이어지는 선
        ColLeft,        // 첫 번째 열을 모두 채운 경우
        ColRight,       // 두 번째 열을 모두 채운 경우
        RowTop,         // 세 번째 열을 모두 채운 경우
        RowMiddle,      // 두 번째 행을 모두 채운 경우
        RowBottom,      // 세 번째 행을 모두 채운 경우
    }
}
