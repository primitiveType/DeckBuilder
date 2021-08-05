 -- defines a factorial function
        function fact (n)
            if (n == 0) then
                return 1
            else
                return Mul(n, fact(n - 1));
			
            end
        end;
        function log (n)
            Log(n)
        end;