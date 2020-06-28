-- print("...")

-- for 循环
-- for i=1,10 do
-- end

-- for i,v in ipairs(table_name) do
-- end

-- while 循环
-- while true do  end

-- ipairs 用于 索引
for i,v in ipairs({10,11,12,13}) do
    print(i,v)
end
-- pairs 用于key引导
for key, value in pairs({10,11,12,13}) do
    print(key, value)
end

-- if then end
if not false then print(false) end

local num = 5
-- lua中的 do-while, repeat-until
repeat
    num = num + 1
    print(" repeat "..num)
until num > 10

-- for var=exp1,exp2,exp3 do - end, exp1 变化到 exp2， exp3为步长
for index = 1, 10, 2 do
    print ("index "..index)
end

-- 
local num = tonumber("10")
print("num:"..num)

local floorNum = math.floor( 10.3 )

local uperNum = math.abs( 10 )

local tolerance = 10
function isturnback( angle )
    angle = angle % 360
    return math.abs( angle )
end

-- ==, ~=
local equal = 10 == 0
local unequal = 10 ~= 0

-- and, or, not
local isTrue = true and true
local isFalse = false or false
local notTrue = not false

print(notTrue)



-- table
days = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
-- 初始化 table
polyline = { color = "blue", npoints = 4, pos = {x = 0, y = 0}}
-- lua的索引从1开始

-- 赋值
a, b, c = 0, 0, 0

-- do-end 模块
do
    local a2 = 0
    local a3 = 0
    print( a2, a3 )
end

foo = 1
do
    local foo = foo
    foo = 3
    print("local foo "..foo)
end
print("global foo ".. foo)


function getNums( )
    return 1, 2, 3
end

local one, two, three = getNums();

print(one, two, three)

-- table.unpack 依次返回数组
local t = {"a","b","c","d"}
local x, y, z = table.unpack(t)
print(x, y, z)

-- 用 select 来访问变长参数 返回第 index个 到最后的参数
function add(...)
    for i = 1, select('#', ...) do
        local arg = select(i, ...)
        print(arg)
    end
    -- print("add" .. ...)
end

add(1,2,10)

-- table.sort 排序
network = {{name = "xxx"}, {name = "xx2"}}
table.sort(network, function ( a, b )
    return a.name > b.name
end )

-- 返回函数
function testRFunc( ... )
    local time = 10
    return function(timeA) return timeA > time end;
end
local func = testRFunc()
print(func(1))

Lib = {}
Lib.foo = function(x, y) return x + y end

local g = function() return "func" end;

function values(t)
    local i = 0
    return function() i = i + 1; return t[i] end;
end

-- yield 就是挂起在下次调用时 继续
local coroutineFunc = function()
    for i = 1, 10, 1 do
        print("co ".. i)
        coroutine.yield()
    end
end

-- 协程
co = coroutine.create( function() coroutineFunc() end)
print(co)
print(coroutine.status( co ))
coroutine.resume( co )
coroutine.resume( co )
coroutine.resume( co )


co1 = coroutine.create( function(a, b, c) 
    print("co1", a, b, c)
    coroutine.yield( a + b + c )
    return a, b, c
end )
-- 在 resume 中传参数 返回值 为true 表示没有错误 后面跟上 第一次yield 的返回值
print(coroutine.resume( co1, 1, 2, 3 ))
-- 返回再一次的yield 值， 或者 函数返回值
print(coroutine.resume( co1, 1, 2, 3 ))