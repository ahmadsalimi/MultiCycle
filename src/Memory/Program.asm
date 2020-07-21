li      $0, 10
li      $1, 4
add     $2, $1, $0, 1   # $2 = $1 + $0 = 15 (all alu signals = 0)
sub     $3, $2, $1		# $3 = $2 - $1 = 11 (all alu signals = 0)
slt     $4, $3, $2      # $4 = $3 < $2 = 1 (all alu signals = 0)
noop
sll     $5, $4, 3       # $5 = $4 << 3 = 8 (all alu signals = 0)
srl     $6, $5, 2       # $6 = $5 >> 2 = 2 (all alu signals = 0)
nand    $7, $5, $6      # $7 = $5 nand $6 = -1 (sgn = 1)
min     $8, $6, $7      # $8 = min($6, $7) = -1 (sgn = 1)
sll     $9, $4, 30      # $9 = 1 << 30 = 2 ^ 30 (all alu signals = 0)
add     $10, $9, $9     # $10 = $9 + $9 = - 2 ^ 31 (overflow, eq, sgn = 1)
sub     $11, $9, $9     # $11 = 0 (eq, zero = 1)

# finish
